using Clinic.Services.Application.Interfaces;
using Clinic.Services.Domain.Entities;
using Clinic.Services.Domain.Enums;
using Clinic.Services.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Services.Infrastructure.Repositories;

public class ServiceItemRepository : IServiceItemRepository
{
    private readonly ServicesDbContext _context;

    public ServiceItemRepository(ServicesDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceItem?> GetByIdAsync(Guid id)
    {
        return await _context.ServiceItems
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<ServiceItem>> GetBySpecializationIdAsync(Guid specializationId)
    {
        return await _context.ServiceItems
            .Where(s => s.SpecializationId == specializationId)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<List<ServiceItem>> GetActiveByCategoryAsync(ServiceCategory category)
    {
        return await _context.ServiceItems
            .Include(s => s.Specialization)
            .Where(s =>
                s.Category == category &&
                s.Status == CatalogStatus.Active)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task AddAsync(ServiceItem service)
    {
        await _context.ServiceItems.AddAsync(service);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ServiceItem service)
    {
        _context.ServiceItems.Update(service);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<ServiceItem> services)
    {
        _context.ServiceItems.UpdateRange(services);
        await _context.SaveChangesAsync();
    }
}
