using Clinic.Services.Application.DTOs.Services;
using Clinic.Services.Domain.Entities;

namespace Clinic.Services.Application.Mappings;

public static class ServiceItemMappingExtensions
{
    public static ServiceResponseDto ToResponseDto(this ServiceItem service)
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
