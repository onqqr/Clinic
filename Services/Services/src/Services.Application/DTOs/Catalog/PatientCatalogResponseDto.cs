using Clinic.Services.Application.DTOs.Services;

namespace Clinic.Services.Application.DTOs.Catalog;

public record PatientCatalogResponseDto
{
    public List<ConsultationGroupDto> Consultations { get; init; } = [];

    public List<ServiceResponseDto> Diagnostics { get; init; } = [];

    public List<ServiceResponseDto> Analyses { get; init; } = [];
}

public record ConsultationGroupDto
{
    public Guid SpecializationId { get; init; }

    public string SpecializationName { get; init; } = string.Empty;

    public List<ServiceResponseDto> Services { get; init; } = [];
}
