using Clinic.Services.Application.DTOs.Services;
using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Application.DTOs.Specializations;

public record SpecializationResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public CatalogStatus Status { get; init; }

    public List<ServiceResponseDto> Services { get; init; } = [];
}
