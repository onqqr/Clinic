using AutoMapper;
using Clinic.Profile.Application.DTOs.Receptionists;
using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Mapping;

public class ReceptionistMappingProfile : AutoMapper.Profile
{
    public ReceptionistMappingProfile()
    {
        CreateMap<Receptionist, ReceptionistResponseDto>()
            .ForMember(dest => dest.OfficeAddress, opt => opt.Ignore());

        CreateMap<CreateReceptionistRequestDto, Receptionist>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateReceptionistRequestDto, Receptionist>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
