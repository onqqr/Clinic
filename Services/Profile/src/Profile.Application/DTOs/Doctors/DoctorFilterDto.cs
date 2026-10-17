namespace Clinic.Profile.Application.DTOs.Doctors;

public record DoctorFilterDto
{
    public string? FullName { get; init; }

    public string? Specialization { get; init; }

    public Guid? OfficeId { get; init; }

    public bool OnlyAvailable { get; init; }
}
