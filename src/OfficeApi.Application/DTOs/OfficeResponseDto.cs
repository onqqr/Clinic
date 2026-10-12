namespace OfficeApi.Application.DTOs;

using OfficeApi.Domain.Enums;

public class OfficeResponseDto
{
    public Guid Id { get; set; }

    public string? PhotoUrl { get; set; }

    public string Address { get; set; } = string.Empty;

    public OfficeStatus Status { get; set; }

    public string RegistryPhoneNumber { get; set; } = string.Empty;
}