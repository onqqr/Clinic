using Clinic.Services.Application.DTOs.Catalog;
using Clinic.Services.Application.DTOs.Services;
using Clinic.Services.Application.DTOs.Specializations;
using Clinic.Services.Application.Events;
using Clinic.Services.Application.Interfaces;
using Clinic.Services.Application.Mappings;
using Clinic.Services.Domain.Entities;
using Clinic.Services.Domain.Enums;

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
        if (await _specializationRepository.ExistsByNameAsync(request.Name))
        {
            throw new InvalidOperationException(
                "Specialization with this name already exists.");
        }

        EnsureServicesMatchSpecializationStatus(request.Status, request.Services);

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

        if (await _specializationRepository.ExistsByNameAsync(request.Name, id))
        {
            throw new InvalidOperationException(
                "Specialization with this name already exists.");
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

        var consultationGroups = consultations
            .GroupBy(s => new { s.SpecializationId, Name = s.Specialization!.Name })
            .Select(g => new ConsultationGroupDto
            {
                SpecializationId = g.Key.SpecializationId,
                SpecializationName = g.Key.Name,
                Services = g.Select(s => s.ToResponseDto()).ToList()
            })
            .OrderBy(g => g.SpecializationName)
            .ToList();

        return new PatientCatalogResponseDto
        {
            Consultations = consultationGroups,
            Diagnostics = diagnostics.Select(s => s.ToResponseDto()).ToList(),
            Analyses = analyses.Select(s => s.ToResponseDto()).ToList()
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
            new SpecializationDeactivated(
                specialization.Id,
                specialization.Name));
    }

    private static void EnsureServicesMatchSpecializationStatus(
        CatalogStatus specializationStatus,
        IEnumerable<CreateServiceRequestDto> services)
    {
        if (specializationStatus != CatalogStatus.Active &&
            services.Any(s => s.Status == CatalogStatus.Active))
        {
            throw new InvalidOperationException(
                "Cannot create an active service under an inactive specialization.");
        }
    }

    private static SpecializationResponseDto MapSpecialization(Specialization specialization)
    {
        return new SpecializationResponseDto
        {
            Id = specialization.Id,
            Name = specialization.Name,
            Status = specialization.Status,
            Services = specialization.Services.Select(s => s.ToResponseDto()).ToList()
        };
    }
}
