using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Application.DTOs.Services;

public record ChangeServiceStatusRequestDto
{
    public CatalogStatus Status { get; init; }
}
