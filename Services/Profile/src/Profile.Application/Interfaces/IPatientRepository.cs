using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Interfaces;

public interface IPatientRepository
{
    Task<List<Patient>> GetAllAsync(string? fullName = null);
    
    Task<Patient?> GetByIdAsync(Guid id);
    
    Task AddAsync(Patient patient);
    
    Task UpdateAsync(Patient patient);
    
    Task DeleteAsync(Patient patient);

    Task<List<Patient>> GetUnlinkedAsync();
}