using Clinic.Services.Application.DTOs.Services;
using Clinic.Services.Application.Interfaces;
using Clinic.Services.Domain.Entities;

namespace Clinic.Services.Application.Services;

public class ServiceItemService : IServiceItemService
{
    private readonly IServiceItemRepository _serviceItemRepository;
    private readonly ISpecializationRepository _specializationRepository;

    public ServiceItemService(
        IServiceItemRepository serviceItemRepository,
        ISpecializationRepository specializationRepository)
    {
        _serviceItemRepository = serviceItemRepository;
        _specializationRepository = specializationRepository;
    }

    public async Task<ServiceResponseDto?> GetByIdAsync(Guid id)
    {
        var service = await _serviceItemRepository.GetByIdAsync(id);

        return service is null ? null : Map(service);
    }

    public async Task<ServiceResponseDto?> CreateAsync(
        Guid specializationId,
        CreateServiceRequestDto request)
    {
        var specialization = await _specializationRepository.GetByIdAsync(
            specializationId,
            includeServices: false);

        if (specialization is null)
        {
            return null;
        }

        var service = new ServiceItem
        {
            Id = Guid.NewGuid(),
            SpecializationId = specializationId,
            Name = request.Name,
            Price = request.Price,
            Category = request.Category,
            Status = request.Status
        };

        await _serviceItemRepository.AddAsync(service);

        return Map(service);
    }

    public async Task<ServiceResponseDto?> UpdateAsync(
        Guid id,
        UpdateServiceRequestDto request)
    {
        var service = await _serviceItemRepository.GetByIdAsync(id);

        if (service is null)
        {
            return null;
        }

        service.Name = request.Name;
        service.Price = request.Price;
        service.Category = request.Category;
        service.Status = request.Status;

        await _serviceItemRepository.UpdateAsync(service);

        return Map(service);
    }

    public async Task<ServiceResponseDto?> ChangeStatusAsync(
        Guid id,
        ChangeServiceStatusRequestDto request)
    {
        var service = await _serviceItemRepository.GetByIdAsync(id);

        if (service is null)
        {
            return null;
        }

        service.Status = request.Status;
        await _serviceItemRepository.UpdateAsync(service);

        return Map(service);
    }

    private static ServiceResponseDto Map(ServiceItem service)
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
