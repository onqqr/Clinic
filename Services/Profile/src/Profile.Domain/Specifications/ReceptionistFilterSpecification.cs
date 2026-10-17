using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Domain.Specifications;

public sealed class ReceptionistFilterSpecification : Specification<Receptionist>
{
    public ReceptionistFilterSpecification(string? fullName = null)
    {
        ApplyOrderBy(r => r.LastName);
        ApplyThenBy(r => r.FirstName);

        if (!string.IsNullOrWhiteSpace(fullName))
        {
            var search = fullName.Trim().ToLower();
            AddCriteria(r =>
                (r.FirstName + " " + r.LastName + " " + (r.MiddleName ?? ""))
                    .Trim()
                    .ToLower()
                    .Contains(search));
        }
    }
}
