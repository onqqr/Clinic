namespace Clinic.Profile.Application.Events;

public record ReceptionistProfileCreated(Guid ReceptionistId, string Email);