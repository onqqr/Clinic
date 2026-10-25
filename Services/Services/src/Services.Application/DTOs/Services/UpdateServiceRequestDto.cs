using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Application.DTOs.Services;

public record UpdateServiceRequestDto
{
    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public ServiceCategory Category { get; init; }

    public CatalogStatus Status { get; init; }
}
