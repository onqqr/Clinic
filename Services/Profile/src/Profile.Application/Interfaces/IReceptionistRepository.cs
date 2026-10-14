using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Interfaces;

public interface IReceptionistRepository
{
    Task<List<Receptionist>> GetAllAsync(string? fullName = null);
    
    Task<Receptionist?> GetByIdAsync(Guid id);
    
    Task AddAsync(Receptionist receptionist);
    
    Task UpdateAsync(Receptionist receptionist);
    
    Task DeleteAsync(Receptionist receptionist);
    
    Task<bool> EmailExistsAsync(string email, Guid? excludeReceptionistId = null);
}