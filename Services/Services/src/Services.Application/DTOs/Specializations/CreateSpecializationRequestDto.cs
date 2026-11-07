using Clinic.Services.Application.DTOs.Services;
using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Application.DTOs.Specializations;

public record CreateSpecializationRequestDto
{
    public string Name { get; init; } = string.Empty;

    public CatalogStatus Status { get; init; } = CatalogStatus.Active;

    public List<CreateServiceRequestDto> Services { get; init; } = [];
}
