using Microsoft.EntityFrameworkCore;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Enums;
using ProfilesApi.Infrastructure.Data;

namespace ProfilesApi.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ProfilesDbContext _dbContext;
    
    public DoctorRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Doctor>> GetAllAsync(
        string? fullName = null,
        string? specialization = null,
        Guid? officeId = null,
        bool onlyAvailable = false)
    {
        var query = _dbContext.Doctors.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(fullName))
        {
            var search = fullName.Trim();

            query = query.Where(d =>
                EF.Functions.ILike(
                    (d.FirstName + " " + d.LastName + " " +
                     (d.MiddleName ?? "")).Trim(),
                    $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(specialization))
        {
            query = query.Where(d =>
                EF.Functions.ILike(d.Specialization, specialization.Trim()));
        }

        if (officeId.HasValue)
        {
            query = query.Where(d => d.OfficeId == officeId.Value);
        }

        if (onlyAvailable)
        {
            query = query.Where(d => d.Status == DoctorStatus.AtWork);
        }

        return await query.OrderBy(d => d.LastName).ToListAsync();
    }

    public Task<Doctor?> GetByIdAsync(Guid id)
    {
        return _dbContext.Doctors.FirstOrDefaultAsync(d => d.Id == id);
    }

    public Task<Doctor?> GetByEmailAsync(string email)
    {
        return _dbContext.Doctors.FirstOrDefaultAsync(
            d => d.Email.ToLower() == email.Trim().ToLower());
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