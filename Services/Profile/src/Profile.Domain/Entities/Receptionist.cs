namespace Clinic.Profile.Domain.Entities;

public class Receptionist
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? PhotoUrl { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public string Email { get; set; } = string.Empty;

    public Guid OfficeId { get; set; }
}