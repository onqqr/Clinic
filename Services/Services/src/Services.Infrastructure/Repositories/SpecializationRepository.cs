using Clinic.Services.Application.Interfaces;
using Clinic.Services.Domain.Entities;
using Clinic.Services.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Services.Infrastructure.Repositories;

public class SpecializationRepository : ISpecializationRepository
{
    private readonly ServicesDbContext _context;

    public SpecializationRepository(ServicesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Specialization>> GetAllAsync()
    {
        return await _context.Specializations
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Specialization?> GetByIdAsync(Guid id, bool includeServices = true)
    {
        IQueryable<Specialization> query = _context.Specializations;

        if (includeServices)
        {
            query = query.Include(s => s.Services);
        }

        return await query.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Specialization specialization)
    {
        await _context.Specializations.AddAsync(specialization);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Specialization specialization)
    {
        _context.Specializations.Update(specialization);
        await _context.SaveChangesAsync();
    }
}
