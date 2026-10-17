namespace Clinic.Profile.Application.DTOs.Patients;

public record ConfirmPatientMatchRequestDto
{
    public Guid MatchedPatientId { get; init; }

    public bool IsMatchConfirmed { get; init; }

    public RegisterPatientRequestDto RegistrationData { get; init; } = new();
}
