using ProfilesApi.Domain.Enums;

namespace ProfilesApi.Application.DTOs.Doctors;

public class DoctorResponseDto
{
    public Guid Id { get; set; }
    
    public string? PhotoUrl { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string? MiddleName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string Specialization { get; set; } = string.Empty;
    
    public Guid OfficeId { get; set; }
    
    public int CareerStartYear { get; set; }
    
    public int Experience { get; set; }
    
    public DoctorStatus Status { get; set; }
}