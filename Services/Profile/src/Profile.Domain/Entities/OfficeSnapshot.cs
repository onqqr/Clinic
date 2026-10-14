namespace Clinic.Profile.Domain.Entities;

public class OfficeSnapshot
{
    public Guid Id { get; set; }
    
    public string Address { get; set; } = string.Empty;

    public string Status { get; set; } = "Active";
}