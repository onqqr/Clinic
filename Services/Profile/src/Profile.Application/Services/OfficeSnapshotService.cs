using Clinic.Profile.Application.DTOs.Offices;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Services;

public class OfficeSnapshotService : IOfficeSnapshotService
{
    private readonly IOfficeSnapshotRepository _repository;

    public OfficeSnapshotService(IOfficeSnapshotRepository repository)
    {
        _repository = repository;
    }

    public async Task<OfficeSnapshotResponseDto?> GetByIdAsync(Guid id)
    {
        var snapshot = await _repository.GetByIdAsync(id);
        return snapshot is null ? null : MapToDto(snapshot);
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

        await _repository.UpsertAsync(snapshot);
        return MapToDto(snapshot);
    }

    private static OfficeSnapshotResponseDto MapToDto(OfficeSnapshot snapshot) => new()
    {
        Id = snapshot.Id,
        Address = snapshot.Address,
        Status = snapshot.Status
    };
}