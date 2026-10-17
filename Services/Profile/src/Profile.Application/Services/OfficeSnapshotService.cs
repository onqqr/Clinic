using Clinic.Profile.Application.DTOs.Offices;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Interfaces;

namespace Clinic.Profile.Application.Services;

public class OfficeSnapshotService : IOfficeSnapshotService
{
    private readonly IOfficeSnapshotRepository _officeSnapshotRepository;

    public OfficeSnapshotService(IOfficeSnapshotRepository officeSnapshotRepository)
    {
        _officeSnapshotRepository = officeSnapshotRepository;
    }

    public async Task<OfficeSnapshotResponseDto?> GetByIdAsync(Guid id)
    {
        var snapshot = await _officeSnapshotRepository.GetByIdAsync(id);

        if (snapshot is null)
        {
            return null;
        }

        return MapToDto(snapshot);
    }

    public async Task<OfficeSnapshotResponseDto> UpsertAsync(
        UpsertOfficeSnapshotRequestDto request)
    {
        var snapshot = new OfficeSnapshot
        {
            Id = request.Id,
            Address = request.Address,
            Status = request.Status
        };

        await _officeSnapshotRepository.UpsertAsync(snapshot);

        return MapToDto(snapshot);
    }

    private static OfficeSnapshotResponseDto MapToDto(OfficeSnapshot snapshot)
    {
        return new OfficeSnapshotResponseDto
        {
            Id = snapshot.Id,
            Address = snapshot.Address,
            Status = snapshot.Status
        };
    }
}
