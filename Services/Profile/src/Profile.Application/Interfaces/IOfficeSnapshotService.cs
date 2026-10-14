using Clinic.Profile.Application.DTOs.Offices;

namespace Clinic.Profile.Application.Interfaces;

public interface IOfficeSnapshotService
{
    Task<OfficeSnapshotResponseDto?> GetByIdAsync(Guid id);
    Task<OfficeSnapshotResponseDto>  UpsertAsync(UpsertOfficeSnapshotRequestDto request);
}