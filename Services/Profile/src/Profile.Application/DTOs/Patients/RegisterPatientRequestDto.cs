namespace Clinic.Profile.Application.DTOs.Patients;

public class RegisterPatientRequestDto
{
    public string? PhotoUrl { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string? MiddleName { get; set; }
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public DateOnly DateOfBirth { get; set; }
}