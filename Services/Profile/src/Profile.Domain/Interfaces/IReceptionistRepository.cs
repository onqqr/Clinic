using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Specifications;

namespace Clinic.Profile.Domain.Interfaces;

public interface IReceptionistRepository
{
    Task<List<Receptionist>> ListAsync(ISpecification<Receptionist> specification);

    Task<Receptionist?> GetByIdAsync(Guid id);

    Task AddAsync(Receptionist receptionist);

    Task UpdateAsync(Receptionist receptionist);

    Task DeleteAsync(Receptionist receptionist);

    Task<bool> EmailExistsAsync(string email, Guid? excludeReceptionistId = null);
}
