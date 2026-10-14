namespace Clinic.Profile.Application.DTOs.Patients;

public class ConfirmPatientMatchRequestDto
{
    public Guid MatchedPatientId { get; set; }
    
    public bool IsMatchConfirmed { get; set; }
    
    public RegisterPatientRequestDto RegistrationData { get; set; } = new();

}