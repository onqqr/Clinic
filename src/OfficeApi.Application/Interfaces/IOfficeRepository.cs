using OfficeApi.Domain.Entities;

namespace OfficeApi.Application.Interfaces;

public interface IOfficeRepository
{
    Task<List<Office>> GetAllAsync();

    Task<Office?> GetByIdAsync(Guid id);

    Task AddAsync(Office office);

    Task UpdateAsync(Office office);

    Task DeleteAsync(Office office);
}
