using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Application.DTOs.Specializations;

public record SpecializationListItemDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public CatalogStatus Status { get; init; }
}
