namespace OfficeApi.Application.DTOs;

using OfficeApi.Domain.Enums;

public class ChangeOfficeStatusRequestDto
{
    public OfficeStatus Status { get; set; }
}