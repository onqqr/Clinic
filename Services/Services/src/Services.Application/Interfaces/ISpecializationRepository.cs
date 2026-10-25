using Clinic.Services.Domain.Entities;

namespace Clinic.Services.Application.Interfaces;

public interface ISpecializationRepository
{
    Task<List<Specialization>> GetAllAsync();

    Task<Specialization?> GetByIdAsync(Guid id, bool includeServices = true);

    Task AddAsync(Specialization specialization);

    Task UpdateAsync(Specialization specialization);
}
