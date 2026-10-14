namespace Clinic.Profile.Application.DTOs.Patients;

public class PatientResponseDto
{
    public Guid Id { get; set; }
    
    public string? PhotoUrl { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string? MiddleName { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public DateOnly DateOfBirth { get; set; }
    
    public bool IsLinkedToAccount { get; set; }
}