using Clinic.Services.Application.DTOs.Catalog;
using Clinic.Services.Application.DTOs.Services;
using Clinic.Services.Application.DTOs.Specializations;
using Clinic.Services.Application.Events;
using Clinic.Services.Application.Interfaces;
using Clinic.Services.Domain.Entities;
using Clinic.Services.Domain.Enums;
using Clinic.Services.Domain.Interfaces;

namespace Clinic.Services.Application.Services;

public class SpecializationService : ISpecializationService
{
    private readonly ISpecializationRepository _specializationRepository;
    private readonly IServiceItemRepository _serviceItemRepository;
    private readonly IEventPublisher _eventPublisher;

    public SpecializationService(
        ISpecializationRepository specializationRepository,
        IServiceItemRepository serviceItemRepository,
        IEventPublisher eventPublisher)
    {
        _specializationRepository = specializationRepository;
        _serviceItemRepository = serviceItemRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<List<SpecializationListItemDto>> GetAllAsync()
    {
        var items = await _specializationRepository.GetAllAsync();

        return items
            .Select(s => new SpecializationListItemDto
            {
                Id = s.Id,
                Name = s.Name,
                Status = s.Status
            })
            .ToList();
    }

    public async Task<SpecializationResponseDto?> GetByIdAsync(Guid id)
    {
        var specialization = await _specializationRepository.GetByIdAsync(id);

        return specialization is null ? null : MapSpecialization(specialization);
    }

    public async Task<SpecializationResponseDto> CreateAsync(
        CreateSpecializationRequestDto request)
    {
        var specialization = new Specialization
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Status = request.Status,
            Services = request.Services
                .Select(s => new ServiceItem
                {
                    Id = Guid.NewGuid(),
                    Name = s.Name,
                    Price = s.Price,
                    Category = s.Category,
                    Status = s.Status
                })
                .ToList()
        };

        await _specializationRepository.AddAsync(specialization);

        return MapSpecialization(specialization);
    }

    public async Task<SpecializationResponseDto?> UpdateAsync(
        Guid id,
        UpdateSpecializationRequestDto request)
    {
        var specialization = await _specializationRepository.GetByIdAsync(id);

        if (specialization is null)
        {
            return null;
        }

        var previousStatus = specialization.Status;
        specialization.Name = request.Name;
        specialization.Status = request.Status;

        if (request.Status == CatalogStatus.Inactive &&
            previousStatus != CatalogStatus.Inactive)
        {
            await DeactivateServicesAndPublishAsync(specialization);
        }
        else
        {
            await _specializationRepository.UpdateAsync(specialization);
        }

        return MapSpecialization(specialization);
    }

    public async Task<SpecializationResponseDto?> ChangeStatusAsync(
        Guid id,
        ChangeSpecializationStatusRequestDto request)
    {
        var specialization = await _specializationRepository.GetByIdAsync(id);

        if (specialization is null)
        {
            return null;
        }

        var previousStatus = specialization.Status;
        specialization.Status = request.Status;

        if (request.Status == CatalogStatus.Inactive &&
            previousStatus != CatalogStatus.Inactive)
        {
            await DeactivateServicesAndPublishAsync(specialization);
        }
        else
        {
            await _specializationRepository.UpdateAsync(specialization);
        }

        return MapSpecialization(specialization);
    }

    public async Task<PatientCatalogResponseDto> GetPatientCatalogAsync()
    {
        var consultations = await _serviceItemRepository
            .GetActiveByCategoryAsync(ServiceCategory.Consultation);
        var diagnostics = await _serviceItemRepository
            .GetActiveByCategoryAsync(ServiceCategory.Diagnostics);
        var analyses = await _serviceItemRepository
            .GetActiveByCategoryAsync(ServiceCategory.Analyses);

        var activeConsultationGroups = consultations
            .Where(s => s.Specialization is { Status: CatalogStatus.Active })
            .GroupBy(s => new { s.SpecializationId, Name = s.Specialization!.Name })
            .Select(g => new ConsultationGroupDto
            {
                SpecializationId = g.Key.SpecializationId,
                SpecializationName = g.Key.Name,
                Services = g.Select(MapService).ToList()
            })
            .OrderBy(g => g.SpecializationName)
            .ToList();

        return new PatientCatalogResponseDto
        {
            Consultations = activeConsultationGroups,
            Diagnostics = diagnostics.Select(MapService).ToList(),
            Analyses = analyses.Select(MapService).ToList()
        };
    }

    private async Task DeactivateServicesAndPublishAsync(Specialization specialization)
    {
        foreach (var service in specialization.Services)
        {
            service.Status = CatalogStatus.Inactive;
        }

        await _specializationRepository.UpdateAsync(specialization);

        await _eventPublisher.PublishAsync(
            new SpecializationStatusChanged(
                specialization.Id,
                specialization.Name,
                CatalogStatus.Inactive));
    }

    private static SpecializationResponseDto MapSpecialization(Specialization specialization)
    {
        return new SpecializationResponseDto
        {
            Id = specialization.Id,
            Name = specialization.Name,
            Status = specialization.Status,
            Services = specialization.Services.Select(MapService).ToList()
        };
    }

    private static ServiceResponseDto MapService(ServiceItem service)
    {
        return new ServiceResponseDto
        {
            Id = service.Id,
            SpecializationId = service.SpecializationId,
            Name = service.Name,
            Price = service.Price,
            Category = service.Category,
            Status = service.Status
        };
    }
}
