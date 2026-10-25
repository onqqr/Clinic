using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Domain.Entities;

public class ServiceItem
{
    public Guid Id { get; set; }

    public Guid SpecializationId { get; set; }

    public Specialization? Specialization { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public ServiceCategory Category { get; set; }

    public CatalogStatus Status { get; set; } = CatalogStatus.Active;
}
