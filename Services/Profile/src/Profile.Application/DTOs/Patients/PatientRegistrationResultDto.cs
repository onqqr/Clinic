using Clinic.Profile.Application.Enums;

namespace Clinic.Profile.Application.DTOs.Patients;

public class PatientRegistrationResultDto
{
    public PatientRegistrationOutcome Outcome { get; set; }

    public PatientResponseDto? Patient { get; set; }

    public PatientResponseDto? MatchedPatient { get; set; }
}
