using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Interfaces;
using Clinic.Profile.Domain.Specifications;
using Clinic.Profile.Infrastructure.Data;
using Clinic.Profile.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Profile.Infrastructure.Repositories;

public class ReceptionistRepository : IReceptionistRepository
{
    private readonly ProfilesDbContext _dbContext;

    public ReceptionistRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Receptionist>> ListAsync(ISpecification<Receptionist> specification)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbContext.Receptionists.AsQueryable(),
            specification);

        return await query.ToListAsync();
    }

    public Task<Receptionist?> GetByIdAsync(Guid id)
    {
        return _dbContext.Receptionists.FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<bool> EmailExistsAsync(
        string email,
        Guid? excludeReceptionistId = null)
    {
        var normalizedEmail = email.Trim().ToLower();

        return _dbContext.Receptionists.AnyAsync(d =>
            d.Email.ToLower() == normalizedEmail &&
            (!excludeReceptionistId.HasValue || d.Id != excludeReceptionistId.Value));
    }

    public async Task AddAsync(Receptionist receptionist)
    {
        await _dbContext.Receptionists.AddAsync(receptionist);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Receptionist receptionist)
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Receptionist receptionist)
    {
        _dbContext.Receptionists.Remove(receptionist);
        await _dbContext.SaveChangesAsync();
    }
}
