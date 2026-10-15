namespace Clinic.Profile.Application.Events;

public record DoctorProfileCreated(Guid DoctorId, string Email);