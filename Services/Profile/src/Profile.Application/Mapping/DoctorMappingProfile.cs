using AutoMapper;
using Clinic.Profile.Application.DTOs.Doctors;
using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Mapping;

public class DoctorMappingProfile : AutoMapper.Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<Doctor, DoctorResponseDto>()
            .ForMember(
                dest => dest.Experience,
                opt => opt.MapFrom(src =>
                    DateTime.Today.Year - src.CareerStartYear + 1))
            .ForMember(
                dest => dest.OfficeAddress,
                opt => opt.Ignore());

        CreateMap<CreateDoctorRequestDto, Doctor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateDoctorRequestDto, Doctor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
