namespace Clinic.Profile.Application.DTOs.Offices;

public record UpsertOfficeSnapshotRequestDto
{
    public Guid Id { get; init; }

    public string Address { get; init; } = string.Empty;

    public string Status { get; init; } = "Active";
}
