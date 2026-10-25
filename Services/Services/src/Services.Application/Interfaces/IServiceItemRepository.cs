using Clinic.Services.Domain.Entities;
using Clinic.Services.Domain.Enums;

namespace Clinic.Services.Application.Interfaces;

public interface IServiceItemRepository
{
    Task<ServiceItem?> GetByIdAsync(Guid id);

    Task<List<ServiceItem>> GetBySpecializationIdAsync(Guid specializationId);

    Task<List<ServiceItem>> GetActiveByCategoryAsync(ServiceCategory category);

    Task AddAsync(ServiceItem service);

    Task UpdateAsync(ServiceItem service);

    Task UpdateRangeAsync(IEnumerable<ServiceItem> services);
}
