namespace Clinic.Profile.Application.DTOs.Receptionists;

public record CreateReceptionistRequestDto
{
    public string? PhotoUrl { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string? MiddleName { get; init; }

    public string Email { get; init; } = string.Empty;

    public Guid OfficeId { get; init; }
}
