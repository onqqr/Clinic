namespace Clinic.Profile.Application.DTOs.Patients;

public class PatientRegistrationResultDto
{
    public string Outcome { get; set; } = string.Empty;
    
    public PatientResponseDto? Patient { get; set; }
    
    public PatientResponseDto? MatchedPatient { get; set; }
}