using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Specifications;

namespace Clinic.Profile.Domain.Interfaces;

public interface IPatientRepository
{
    Task<List<Patient>> ListAsync(ISpecification<Patient> specification);

    Task<Patient?> GetByIdAsync(Guid id);

    Task AddAsync(Patient patient);

    Task UpdateAsync(Patient patient);

    Task DeleteAsync(Patient patient);
}
