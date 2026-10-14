namespace Clinic.Profile.Application.DTOs.Receptionist;

public class UpdateReceptionistRequestDto
{
    public string? PhotoUrl { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string? MiddleName { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public Guid OfficeId { get; set; }
}