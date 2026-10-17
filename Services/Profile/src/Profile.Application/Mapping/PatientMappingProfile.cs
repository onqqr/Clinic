using AutoMapper;
using Clinic.Profile.Application.DTOs.Patients;
using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Mapping;

public class PatientMappingProfile : AutoMapper.Profile
{
    public PatientMappingProfile()
    {
        CreateMap<Patient, PatientResponseDto>();

        CreateMap<CreatePatientRequestDto, Patient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PhotoUrl, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
            .ForMember(dest => dest.IsLinkedToAccount, opt => opt.MapFrom(_ => false));

        CreateMap<UpdatePatientRequestDto, Patient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsLinkedToAccount, opt => opt.Ignore());

        CreateMap<RegisterPatientRequestDto, Patient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsLinkedToAccount, opt => opt.MapFrom(_ => true));
    }
}
