using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Domain.Interfaces;

public interface IOfficeSnapshotRepository
{
    Task<OfficeSnapshot?> GetByIdAsync(Guid id);

    Task UpsertAsync(OfficeSnapshot snapshot);
}
