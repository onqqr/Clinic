namespace Clinic.Profile.Application.DTOs.Offices;

public class UpsertOfficeSnapshotRequestDto
{
    public Guid Id { get; set; }
    
    public string Address { get; set; } = string.Empty;
    
    public string Status { get; set; } = "Active";
}