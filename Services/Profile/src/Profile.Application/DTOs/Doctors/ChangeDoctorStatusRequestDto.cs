using Clinic.Profile.Domain.Enums;

namespace Clinic.Profile.Application.DTOs.Doctors;

public class ChangeDoctorStatusRequestDto
{
    public DoctorStatus Status { get; set; }
}