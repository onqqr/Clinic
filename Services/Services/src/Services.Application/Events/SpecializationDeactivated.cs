namespace Clinic.Services.Application.Events;

public record SpecializationDeactivated(
    Guid SpecializationId,
    string SpecializationName);
