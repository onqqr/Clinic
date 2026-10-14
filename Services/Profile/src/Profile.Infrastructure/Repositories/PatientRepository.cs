using Microsoft.EntityFrameworkCore;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Infrastructure.Data;

namespace Clinic.Profile.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ProfilesDbContext _dbContext;

    public PatientRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Patient>> GetAllAsync(string? fullName = null)
    {
        var query = _dbContext.Patients.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(fullName))
        {
            var search = fullName.Trim();

            query = query.Where(p =>
                EF.Functions.ILike(
                    (p.FirstName + " " + p.LastName + " " +
                     (p.MiddleName ?? "")).Trim(),
                    $"%{search}%"));
        }

        return await query
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();
    }

    public Task<Patient?> GetByIdAsync(Guid id)
    {
        return _dbContext.Patients.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Patient patient)
    {
        await _dbContext.Patients.AddAsync(patient);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Patient patient)
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Patient patient)
    {
        _dbContext.Patients.Remove(patient);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Patient>> GetUnlinkedAsync()
    {
        return await  _dbContext.Patients
            .Where(p => !p.IsLinkedToAccount)
            .ToListAsync();
    }
}