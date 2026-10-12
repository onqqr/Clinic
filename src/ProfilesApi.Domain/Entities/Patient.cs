namespace ProfilesApi.Domain.Entities;

public class Patient
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? PhotoUrl { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime DateOfBirth { get; set; }

    public bool IsLinkedToAccount { get; set; } = false;
}