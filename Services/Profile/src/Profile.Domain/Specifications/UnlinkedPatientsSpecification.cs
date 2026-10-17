using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Domain.Specifications;

public sealed class UnlinkedPatientsSpecification : Specification<Patient>
{
    public UnlinkedPatientsSpecification()
    {
        AsNoTracking = false;
        AddCriteria(p => !p.IsLinkedToAccount);
    }
}
