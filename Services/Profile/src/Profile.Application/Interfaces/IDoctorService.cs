using Clinic.Profile.Application.DTOs.Doctors;

namespace Clinic.Profile.Application.Interfaces;

public interface IDoctorService
{
    Task<List<DoctorResponseDto>> GetAllAsync(DoctorFilterDto filter);

    Task<DoctorResponseDto?> GetByIdAsync(Guid id);

    Task<DoctorResponseDto> CreateAsync(CreateDoctorRequestDto request);

    Task<DoctorResponseDto?> UpdateAsync(Guid id, UpdateDoctorRequestDto request);

    Task<bool> DeleteAsync(Guid id);

    Task<DoctorResponseDto?> ChangeStatusAsync(Guid id, ChangeDoctorStatusRequestDto request);
}
