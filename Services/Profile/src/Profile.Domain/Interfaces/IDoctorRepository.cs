using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Domain.Interfaces;

public interface IDoctorRepository
{
    Task<List<Doctor>> GetAllAsync(
        string? fullName = null,
        string? specialization = null,
        Guid? officeId = null,
        bool onlyAvailable = false);

    Task<Doctor?> GetByIdAsync(Guid id);

    Task AddAsync(Doctor doctor);

    Task UpdateAsync(Doctor doctor);

    Task DeleteAsync(Doctor doctor);

    Task<bool> EmailExistsAsync(
        string email,
        Guid? excludeDoctorId = null);
}
