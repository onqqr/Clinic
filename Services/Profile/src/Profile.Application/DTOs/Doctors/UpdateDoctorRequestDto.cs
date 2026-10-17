using Clinic.Profile.Domain.Enums;

namespace Clinic.Profile.Application.DTOs.Doctors;

public record UpdateDoctorRequestDto
{
    public string? PhotoUrl { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string? MiddleName { get; init; }

    public DateOnly DateOfBirth { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Specialization { get; init; } = string.Empty;

    public Guid OfficeId { get; init; }

    public int CareerStartYear { get; init; }

    public DoctorStatus Status { get; init; }
}
