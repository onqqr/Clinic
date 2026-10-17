using AutoMapper;
using Clinic.Profile.Application.DTOs.Patients;
using Clinic.Profile.Application.Enums;
using Clinic.Profile.Application.Events;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Interfaces;
using Clinic.Profile.Domain.Matching;
using Clinic.Profile.Domain.Specifications;

namespace Clinic.Profile.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IMapper _mapper;

    public PatientService(
        IPatientRepository patientRepository,
        IEventPublisher eventPublisher,
        IMapper mapper)
    {
        _patientRepository = patientRepository;
        _eventPublisher = eventPublisher;
        _mapper = mapper;
    }

    public async Task<List<PatientResponseDto>> GetAllAsync(PatientFilterDto filter)
    {
        var specification = new PatientFilterSpecification(filter.FullName);
        var patients = await _patientRepository.ListAsync(specification);

        return _mapper.Map<List<PatientResponseDto>>(patients);
    }

    public async Task<PatientResponseDto?> GetByIdAsync(Guid id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient is null)
        {
            return null;
        }

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<PatientResponseDto> CreateAsync(CreatePatientRequestDto request)
    {
        var patient = _mapper.Map<Patient>(request);
        await _patientRepository.AddAsync(patient);

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<PatientResponseDto?> UpdateAsync(
        Guid id,
        UpdatePatientRequestDto request)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient is null)
        {
            return null;
        }

        _mapper.Map(request, patient);
        await _patientRepository.UpdateAsync(patient);

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient is null)
        {
            return false;
        }

        await _patientRepository.DeleteAsync(patient);
        await _eventPublisher.PublishAsync(new PatientProfileDeleted(id));

        return true;
    }

    public async Task<PatientRegistrationResultDto> RegisterAsync(
        RegisterPatientRequestDto request)
    {
        var unlinked = await _patientRepository.ListAsync(
            new UnlinkedPatientsSpecification());

        Patient? bestMatch = null;
        var bestScore = 0;

        foreach (var candidate in unlinked)
        {
            var score = PatientProfileMatcher.CalculateScore(
                request.FirstName,
                request.LastName,
                request.MiddleName,
                request.DateOfBirth,
                candidate.FirstName,
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
            return new PatientRegistrationResultDto
            {
                Outcome = PatientRegistrationOutcome.MatchFound,
                MatchedPatient = _mapper.Map<PatientResponseDto>(bestMatch)
            };
        }

        var patient = _mapper.Map<Patient>(request);
        await _patientRepository.AddAsync(patient);

        return new PatientRegistrationResultDto
        {
            Outcome = PatientRegistrationOutcome.Created,
            Patient = _mapper.Map<PatientResponseDto>(patient)
        };
    }

    public async Task<PatientRegistrationResultDto> ConfirmMatchAsync(
        ConfirmPatientMatchRequestDto request)
    {
        if (request.IsMatchConfirmed)
        {
            var existing = await _patientRepository.GetByIdAsync(request.MatchedPatientId);

            if (existing is null)
            {
                throw new InvalidOperationException("Matched patient was not found.");
            }

            if (existing.IsLinkedToAccount)
            {
                throw new InvalidOperationException("Patient is already linked to an account.");
            }

            existing.IsLinkedToAccount = true;

            if (!string.IsNullOrWhiteSpace(request.RegistrationData.PhoneNumber))
            {
                existing.PhoneNumber = request.RegistrationData.PhoneNumber;
            }

            if (request.RegistrationData.PhotoUrl is not null)
            {
                existing.PhotoUrl = request.RegistrationData.PhotoUrl;
            }

            await _patientRepository.UpdateAsync(existing);

            return new PatientRegistrationResultDto
            {
                Outcome = PatientRegistrationOutcome.Linked,
                Patient = _mapper.Map<PatientResponseDto>(existing)
            };
        }

        var patient = _mapper.Map<Patient>(request.RegistrationData);
        await _patientRepository.AddAsync(patient);

        return new PatientRegistrationResultDto
        {
            Outcome = PatientRegistrationOutcome.Created,
            Patient = _mapper.Map<PatientResponseDto>(patient)
        };
    }
}
