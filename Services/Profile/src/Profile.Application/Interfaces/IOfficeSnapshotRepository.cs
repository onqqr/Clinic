using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Interfaces;

public interface IOfficeSnapshotRepository
{
    Task<OfficeSnapshot?> GetByIdAsync(Guid id);
    
    Task UpsertAsync(OfficeSnapshot snapshot);
}