using Clinic.Offices.Domain.Enums;

namespace Clinic.Offices.Application.DTOs;


public class ChangeOfficeStatusRequestDto
{
    public OfficeStatus Status { get; set; }
}