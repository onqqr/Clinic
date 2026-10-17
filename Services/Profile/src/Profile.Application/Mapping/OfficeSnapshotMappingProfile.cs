using AutoMapper;
using Clinic.Profile.Application.DTOs.Offices;
using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Mapping;

public class OfficeSnapshotMappingProfile : AutoMapper.Profile
{
    public OfficeSnapshotMappingProfile()
    {
        CreateMap<OfficeSnapshot, OfficeSnapshotResponseDto>();
        CreateMap<UpsertOfficeSnapshotRequestDto, OfficeSnapshot>();
    }
}
