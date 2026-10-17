using Clinic.Profile.Application.Enums;

namespace Clinic.Profile.Application.DTOs.Patients;

public record PatientRegistrationResultDto
{
    public PatientRegistrationOutcome Outcome { get; init; }

    public PatientResponseDto? Patient { get; init; }

    public PatientResponseDto? MatchedPatient { get; init; }
}
