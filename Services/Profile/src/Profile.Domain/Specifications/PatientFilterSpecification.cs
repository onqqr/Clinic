using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Domain.Specifications;

public sealed class PatientFilterSpecification : Specification<Patient>
{
    public PatientFilterSpecification(string? fullName = null)
    {
        ApplyOrderBy(p => p.LastName);
        ApplyThenBy(p => p.FirstName);

        if (!string.IsNullOrWhiteSpace(fullName))
        {
            var search = fullName.Trim().ToLower();
            AddCriteria(p =>
                (p.FirstName + " " + p.LastName + " " + (p.MiddleName ?? ""))
                    .Trim()
                    .ToLower()
                    .Contains(search));
        }
    }
}
