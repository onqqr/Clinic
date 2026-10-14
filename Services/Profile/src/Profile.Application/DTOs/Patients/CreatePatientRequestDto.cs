namespace Clinic.Profile.Application.DTOs.Patients;

public class CreatePatientRequestDto
{
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string? MiddleName { get; set; }
    
    public DateOnly DateOfBirth { get; set; }
}