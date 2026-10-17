using Microsoft.EntityFrameworkCore;
using Clinic.Profile.Domain.Interfaces;
using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Infrastructure.Data;

namespace Clinic.Profile.Infrastructure.Repositories;

public class OfficeSnapshotRepository : IOfficeSnapshotRepository
{
    private readonly ProfilesDbContext _dbContext;

    public OfficeSnapshotRepository(ProfilesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<OfficeSnapshot?> GetByIdAsync(Guid id)
    {
        return _dbContext.OfficeSnapshots
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task UpsertAsync(OfficeSnapshot snapshot)
    {
        var existing = await _dbContext.OfficeSnapshots
            .FirstOrDefaultAsync(o => o.Id == snapshot.Id);

        if (existing is null)
        {
            await _dbContext.OfficeSnapshots.AddAsync(snapshot);
        }
        else
        {
            existing.Address = snapshot.Address;
            existing.Status = snapshot.Status;
        }

        await _dbContext.SaveChangesAsync();
    }
}