using Microsoft.EntityFrameworkCore;
using OfficeApi.Application.Interfaces;
using OfficeApi.Domain.Entities;
using OfficeApi.Infrastructure.Data;

namespace OfficeApi.Infrastructure.Repositories;

public class OfficeRepository : IOfficeRepository
{
    private readonly OfficeDbContext _context;

    public OfficeRepository(OfficeDbContext context)
    {
        _context = context;
    }

    public async Task<List<Office>> GetAllAsync()
    {
        return await _context.Offices.ToListAsync();
    }

    public async Task<Office?> GetByIdAsync(Guid id)
    {
        return await _context.Offices
            .FirstOrDefaultAsync(office => office.Id == id);
    }

    public async Task AddAsync(Office office)
    {
        await _context.Offices.AddAsync(office);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Office office)
    {
        _context.Offices.Update(office);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Office office)
    {
        _context.Offices.Remove(office);
        await _context.SaveChangesAsync();
    }
}
