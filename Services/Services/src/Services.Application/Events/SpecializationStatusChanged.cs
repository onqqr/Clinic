using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Application.Events;

public record SpecializationStatusChanged(
    Guid SpecializationId,
    string SpecializationName,
    CatalogStatus Status);
