namespace Clinic.Profile.Application.Interfaces;

using Clinic.Profile.Application.DTOs.Doctors;

public interface IDoctorService
{
    Task<List<DoctorResponseDto>> GetAllAsync(
        string? fullName = null,
        string? specialization = null,
        Guid? officeId = null,
        bool onlyAvailable = false);

    Task<DoctorResponseDto?> GetByIdAsync(Guid id);

    Task<DoctorResponseDto> CreateAsync(
        CreateDoctorRequestDto request);

    Task<DoctorResponseDto?> UpdateAsync(
        Guid id,
        UpdateDoctorRequestDto request);

    Task<bool> DeleteAsync(Guid id);
    
    Task<DoctorResponseDto?> ChangeStatusAsync(
        Guid id, 
        ChangeDoctorStatusRequestDto request);
}