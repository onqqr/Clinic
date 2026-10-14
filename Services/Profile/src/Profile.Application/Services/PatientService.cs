using Clinic.Profile.Application.DTOs.Patients;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;

    public PatientService(IPatientRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PatientResponseDto>> GetAllAsync(string? fullName = null)
    {
        var patients = await _repository.GetAllAsync(fullName);
        return patients.Select(MapToDto).ToList();
    }

    public async Task<PatientResponseDto?> GetByIdAsync(Guid id)
    {
        var patient = await _repository.GetByIdAsync(id);
        return patient is null ? null : MapToDto(patient);
    }

    public async Task<PatientResponseDto> CreateAsync(CreatePatientRequestDto request)
    {
        // Ресепшионист создаёт «голый» профиль — аккаунт ещё не привязан
        var patient = new Patient
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            DateOfBirth = request.DateOfBirth,
            IsLinkedToAccount = false
        };

        await _repository.AddAsync(patient);
        return MapToDto(patient);
    }

    public async Task<PatientResponseDto?> UpdateAsync(
        Guid id,
        UpdatePatientRequestDto request)
    {
        var patient = await _repository.GetByIdAsync(id);
        if (patient is null)
            return null;

        patient.PhotoUrl = request.PhotoUrl;
        patient.FirstName = request.FirstName;
        patient.LastName = request.LastName;
        patient.MiddleName = request.MiddleName;
        patient.PhoneNumber = request.PhoneNumber;
        patient.DateOfBirth = request.DateOfBirth;

        await _repository.UpdateAsync(patient);
        return MapToDto(patient);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var patient = await _repository.GetByIdAsync(id);
        if (patient is null)
            return false;

        await _repository.DeleteAsync(patient);
        return true;
    }

    private static PatientResponseDto MapToDto(Patient patient)
    {
        return new PatientResponseDto()
        {
            Id = patient.Id,
            PhotoUrl = patient.PhotoUrl,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            MiddleName = patient.MiddleName,
            PhoneNumber = patient.PhoneNumber,
            DateOfBirth = patient.DateOfBirth,
            IsLinkedToAccount = patient.IsLinkedToAccount
        };
    }
}