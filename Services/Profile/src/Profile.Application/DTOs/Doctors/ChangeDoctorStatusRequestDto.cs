using Clinic.Profile.Domain.Enums;

namespace Clinic.Profile.Application.DTOs.Doctors;

public record ChangeDoctorStatusRequestDto
{
    public DoctorStatus Status { get; init; }
}
