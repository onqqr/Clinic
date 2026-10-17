namespace Clinic.Profile.Application.DTOs.Patients;

public record RegisterPatientRequestDto
{
    public string? PhotoUrl { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string? MiddleName { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public DateOnly DateOfBirth { get; init; }
}
