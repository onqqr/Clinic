namespace Clinic.Profile.Application.Interfaces;

using Clinic.Profile.Application.DTOs.Patients;

public interface IPatientService
{
    Task<List<PatientResponseDto>> GetAllAsync(string? fullName = null);
    
    Task<PatientResponseDto?> GetByIdAsync(Guid id);
    
    Task<PatientResponseDto> CreateAsync(CreatePatientRequestDto request);
    
    Task<PatientResponseDto?> UpdateAsync(Guid id, UpdatePatientRequestDto request);
    
    Task<bool> DeleteAsync(Guid id);
    
    Task<PatientRegistrationResultDto> RegisterAsync(RegisterPatientRequestDto request);
    
    Task<PatientRegistrationResultDto> ConfirmMatchAsync(ConfirmPatientMatchRequestDto request);
}