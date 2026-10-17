namespace Clinic.Profile.Application.DTOs.Patients;

public record PatientResponseDto
{
    public Guid Id { get; init; }

    public string? PhotoUrl { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string? MiddleName { get; init; }

    public string? PhoneNumber { get; init; }

    public DateOnly DateOfBirth { get; init; }

    public bool IsLinkedToAccount { get; init; }
}
