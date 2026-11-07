using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Application.DTOs.Specializations;

public record UpdateSpecializationRequestDto
{
    public string Name { get; init; } = string.Empty;

    public CatalogStatus Status { get; init; }
}
