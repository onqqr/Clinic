namespace Clinic.Profile.Application.DTOs.Patients;

public record CreatePatientRequestDto
{
    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string? MiddleName { get; init; }

    public DateOnly DateOfBirth { get; init; }
}
