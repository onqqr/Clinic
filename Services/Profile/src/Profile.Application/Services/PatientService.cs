using Clinic.Profile.Application.DTOs.Patients;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Matching;

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

    public async Task<PatientRegistrationResultDto> RegisterAsync(
        RegisterPatientRequestDto request)
    {
        var unlinked = await _repository.GetUnlinkedAsync();

        Patient? bestMatch = null;
        var bestScore = 0;

        foreach (var candidate in unlinked)
        {
            var score = PatientProfileMatcher.CalculateScore(
                request.FirstName, request.LastName, request.MiddleName,
                request.DateOfBirth, candidate.FirstName, 
                candidate.LastName, 
                candidate.MiddleName,
                candidate.DateOfBirth);

            if (PatientProfileMatcher.IsMatch(score) && score > bestScore)
            {
                bestScore = score;
                bestMatch = candidate;
            }
        }

        if (bestMatch is not null)
        {
            return new PatientRegistrationResultDto()
            {
                Outcome = "MatchFound",
                MatchedPatient = MapToDto(bestMatch)
            };
        }

        var patient = new Patient
        {
            PhotoUrl = request.PhotoUrl,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            PhoneNumber = request.PhoneNumber,
            DateOfBirth = request.DateOfBirth,
            IsLinkedToAccount = true
        };
        
        await _repository.AddAsync(patient);
        return new PatientRegistrationResultDto
        {
            Outcome = "Created",
            Patient = MapToDto(patient)
        };
    }
    
    public async Task<PatientRegistrationResultDto> ConfirmMatchAsync(
        ConfirmPatientMatchRequestDto request)
    {
        if (request.IsMatchConfirmed)
        {
            var existing = await _repository.GetByIdAsync(request.MatchedPatientId);
            if (existing is null)
                throw new InvalidOperationException("Matched patient was not found.");

            if (existing.IsLinkedToAccount)
                throw new InvalidOperationException("Patient is already linked to an account.");

            existing.IsLinkedToAccount = true;
            if (!string.IsNullOrWhiteSpace(request.RegistrationData.PhoneNumber))
                existing.PhoneNumber = request.RegistrationData.PhoneNumber;
            if (request.RegistrationData.PhotoUrl is not null)
                existing.PhotoUrl = request.RegistrationData.PhotoUrl;

            await _repository.UpdateAsync(existing);

            return new PatientRegistrationResultDto
            {
                Outcome = "Linked",
                Patient = MapToDto(existing)
            };
        }
        
        var data = request.RegistrationData;
        var patient = new Patient
        {
            PhotoUrl = data.PhotoUrl,
            FirstName = data.FirstName,
            LastName = data.LastName,
            MiddleName = data.MiddleName,
            PhoneNumber = data.PhoneNumber,
            DateOfBirth = data.DateOfBirth,
            IsLinkedToAccount = true
        };

        await _repository.AddAsync(patient);

        return new PatientRegistrationResultDto
        {
            Outcome = "Created",
            Patient = MapToDto(patient)
        };
    }
}