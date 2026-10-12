namespace ProfilesApi.Domain.Entities;

using ProfilesApi.Domain.Enums;

public class Doctor
{
    public Guid Id { get; set; } =  Guid.NewGuid();
    
    public string? PhotoUrl { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public Guid OfficeId { get; set; }

    public int CareerStartYear { get; set; }

    public DoctorStatus Status { get; set; } = DoctorStatus.AtWork;
}
