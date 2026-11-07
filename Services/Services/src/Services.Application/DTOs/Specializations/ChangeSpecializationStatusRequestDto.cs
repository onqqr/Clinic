using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Application.DTOs.Specializations;

public record ChangeSpecializationStatusRequestDto
{
    public CatalogStatus Status { get; init; }
}
