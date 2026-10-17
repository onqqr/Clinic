using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Interfaces;
using Clinic.Profile.Domain.Specifications;
using Clinic.Profile.Infrastructure.Data;
using Clinic.Profile.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Profile.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ProfilesDbContext _dbContext;

    public PatientRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Patient>> ListAsync(ISpecification<Patient> specification)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbContext.Patients.AsQueryable(),
            specification);

        return await query.ToListAsync();
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
}
