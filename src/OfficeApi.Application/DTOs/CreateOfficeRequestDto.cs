namespace OfficeApi.Application.DTOs;

public class CreateOfficeRequestDto
{
    public string? PhotoUrl { get; set; }

    public string City { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string HouseNumber { get; set; } = string.Empty;

    public string? OfficeNumber { get; set; }

    public string RegistryPhoneNumber { get; set; } = string.Empty;
}
