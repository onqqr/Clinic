namespace OfficeApi.Domain.Entities;

using OfficeApi.Domain.Enums;

public class Office
{
    public Guid Id { get; set; }
    public string? PhotoUrl { get; set; }
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string? OfficeNumber { get; set; }
    public string RegistryPhoneNumber { get; set; } = string.Empty;
    public OfficeStatus Status { get; set; } = OfficeStatus.Active;

    public string Address
    {
        get
        {
            var address = $"{City}, {Street}, {HouseNumber}";
            if (!string.IsNullOrEmpty(OfficeNumber))
                address += $", office {OfficeNumber}";
            return address;
        }
    }
}
