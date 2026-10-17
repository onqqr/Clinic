using Clinic.Profile.Application.DTOs.Patients;

namespace Clinic.Profile.Application.Interfaces;

public interface IPatientService
{
    Task<List<PatientResponseDto>> GetAllAsync(PatientFilterDto filter);

    Task<PatientResponseDto?> GetByIdAsync(Guid id);

    Task<PatientResponseDto> CreateAsync(CreatePatientRequestDto request);

    Task<PatientResponseDto?> UpdateAsync(Guid id, UpdatePatientRequestDto request);

    Task<bool> DeleteAsync(Guid id);

    Task<PatientRegistrationResultDto> RegisterAsync(RegisterPatientRequestDto request);

    Task<PatientRegistrationResultDto> ConfirmMatchAsync(ConfirmPatientMatchRequestDto request);
}
