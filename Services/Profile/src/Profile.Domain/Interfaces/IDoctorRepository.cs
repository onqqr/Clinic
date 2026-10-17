using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Specifications;

namespace Clinic.Profile.Domain.Interfaces;

public interface IDoctorRepository
{
    Task<List<Doctor>> ListAsync(ISpecification<Doctor> specification);

    Task<Doctor?> GetByIdAsync(Guid id);

    Task AddAsync(Doctor doctor);

    Task UpdateAsync(Doctor doctor);

    Task DeleteAsync(Doctor doctor);

    Task<bool> EmailExistsAsync(
        string email,
        Guid? excludeDoctorId = null);
}
