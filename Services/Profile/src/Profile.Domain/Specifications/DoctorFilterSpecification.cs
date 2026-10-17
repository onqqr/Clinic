using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Enums;

namespace Clinic.Profile.Domain.Specifications;

public sealed class DoctorFilterSpecification : Specification<Doctor>
{
    public DoctorFilterSpecification(
        string? fullName = null,
        string? specialization = null,
        Guid? officeId = null,
        bool onlyAvailable = false)
    {
        ApplyOrderBy(d => d.LastName);

        if (!string.IsNullOrWhiteSpace(fullName))
        {
            var search = fullName.Trim().ToLower();
            AddCriteria(d =>
                (d.FirstName + " " + d.LastName + " " + (d.MiddleName ?? ""))
                    .Trim()
                    .ToLower()
                    .Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(specialization))
        {
            var value = specialization.Trim().ToLower();
            AddCriteria(d => d.Specialization.ToLower() == value);
        }

        if (officeId.HasValue)
        {
            var id = officeId.Value;
            AddCriteria(d => d.OfficeId == id);
        }

        if (onlyAvailable)
        {
            AddCriteria(d => d.Status == DoctorStatus.AtWork);
        }
    }
}
