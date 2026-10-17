using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Interfaces;
using Clinic.Profile.Domain.Specifications;
using Clinic.Profile.Infrastructure.Data;
using Clinic.Profile.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Profile.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ProfilesDbContext _dbContext;

    public DoctorRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Doctor>> ListAsync(ISpecification<Doctor> specification)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbContext.Doctors.AsQueryable(),
            specification);

        return await query.ToListAsync();
    }

    public Task<Doctor?> GetByIdAsync(Guid id)
    {
        return _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id);
    }

    public Task<bool> EmailExistsAsync(
        string email,
        Guid? excludeDoctorId = null)
    {
        var normalizedEmail = email.Trim().ToLower();

        return _dbContext.Doctors.AnyAsync(d =>
            d.Email.ToLower() == normalizedEmail &&
            (!excludeDoctorId.HasValue || d.Id != excludeDoctorId.Value));
    }

    public async Task AddAsync(Doctor doctor)
    {
        await _dbContext.Doctors.AddAsync(doctor);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Doctor doctor)
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Doctor doctor)
    {
        _dbContext.Doctors.Remove(doctor);
        await _dbContext.SaveChangesAsync();
    }
}
