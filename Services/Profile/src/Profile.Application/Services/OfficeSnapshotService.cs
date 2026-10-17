using AutoMapper;
using Clinic.Profile.Application.DTOs.Offices;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Interfaces;

namespace Clinic.Profile.Application.Services;

public class OfficeSnapshotService : IOfficeSnapshotService
{
    private readonly IOfficeSnapshotRepository _officeSnapshotRepository;
    private readonly IMapper _mapper;

    public OfficeSnapshotService(
        IOfficeSnapshotRepository officeSnapshotRepository,
        IMapper mapper)
    {
        _officeSnapshotRepository = officeSnapshotRepository;
        _mapper = mapper;
    }

    public async Task<OfficeSnapshotResponseDto?> GetByIdAsync(Guid id)
    {
        var snapshot = await _officeSnapshotRepository.GetByIdAsync(id);

        if (snapshot is null)
        {
            return null;
        }

        return _mapper.Map<OfficeSnapshotResponseDto>(snapshot);
    }

    public async Task<OfficeSnapshotResponseDto> UpsertAsync(
        UpsertOfficeSnapshotRequestDto request)
    {
        var snapshot = _mapper.Map<OfficeSnapshot>(request);
        await _officeSnapshotRepository.UpsertAsync(snapshot);

        return _mapper.Map<OfficeSnapshotResponseDto>(snapshot);
    }
}
