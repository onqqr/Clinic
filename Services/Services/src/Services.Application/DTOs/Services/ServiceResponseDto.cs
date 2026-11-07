using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Application.DTOs.Services;

public record ServiceResponseDto
{
    public Guid Id { get; init; }

    public Guid SpecializationId { get; init; }

    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public ServiceCategory Category { get; init; }

    public CatalogStatus Status { get; init; }
}
