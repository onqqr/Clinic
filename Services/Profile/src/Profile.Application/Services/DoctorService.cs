using Clinic.Profile.Application.DTOs.Doctors;
using Clinic.Profile.Application.Events;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Interfaces;

namespace Clinic.Profile.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IOfficeSnapshotRepository _officeSnapshotRepository;
    private readonly IEventPublisher _eventPublisher;

    public DoctorService(
        IDoctorRepository doctorRepository,
        IOfficeSnapshotRepository officeSnapshotRepository,
        IEventPublisher eventPublisher)
    {
        _doctorRepository = doctorRepository;
        _officeSnapshotRepository = officeSnapshotRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<List<DoctorResponseDto>> GetAllAsync(
        string? fullName = null,
        string? specialization = null,
        Guid? officeId = null,
        bool onlyAvailable = false)
    {
        var doctors = await _doctorRepository.GetAllAsync(
            fullName,
            specialization,
            officeId,
            onlyAvailable);

        var result = new List<DoctorResponseDto>();

        foreach (var doctor in doctors)
        {
            result.Add(await MapToDtoAsync(doctor));
        }

        return result;
    }

    public async Task<DoctorResponseDto?> GetByIdAsync(Guid id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor is null)
        {
            return null;
        }

        return await MapToDtoAsync(doctor);
    }

    public async Task<DoctorResponseDto> CreateAsync(
        CreateDoctorRequestDto request)
    {
        if (await _doctorRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException(
                "User with this email already exists.");
        }

        var doctor = new Doctor
        {
            PhotoUrl = request.PhotoUrl,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            DateOfBirth = request.DateOfBirth,
            Email = request.Email,
            Specialization = request.Specialization,
            OfficeId = request.OfficeId,
            CareerStartYear = request.CareerStartYear,
            Status = request.Status
        };

        await _doctorRepository.AddAsync(doctor);

        await _eventPublisher.PublishAsync(
            new DoctorProfileCreated(doctor.Id, doctor.Email));

        return await MapToDtoAsync(doctor);
    }

    public async Task<DoctorResponseDto?> UpdateAsync(
        Guid id,
        UpdateDoctorRequestDto request)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor is null)
        {
            return null;
        }

        if (await _doctorRepository.EmailExistsAsync(request.Email, id))
        {
            throw new InvalidOperationException(
                "User with this email already exists.");
        }

        doctor.PhotoUrl = request.PhotoUrl;
        doctor.FirstName = request.FirstName;
        doctor.LastName = request.LastName;
        doctor.MiddleName = request.MiddleName;
        doctor.DateOfBirth = request.DateOfBirth;
        doctor.Email = request.Email;
        doctor.Specialization = request.Specialization;
        doctor.OfficeId = request.OfficeId;
        doctor.CareerStartYear = request.CareerStartYear;
        doctor.Status = request.Status;

        await _doctorRepository.UpdateAsync(doctor);

        return await MapToDtoAsync(doctor);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor is null)
        {
            return false;
        }

        await _doctorRepository.DeleteAsync(doctor);

        return true;
    }

    public async Task<DoctorResponseDto?> ChangeStatusAsync(
        Guid id,
        ChangeDoctorStatusRequestDto request)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor is null)
        {
            return null;
        }

        doctor.Status = request.Status;
        await _doctorRepository.UpdateAsync(doctor);

        return await MapToDtoAsync(doctor);
    }

    private async Task<DoctorResponseDto> MapToDtoAsync(Doctor doctor)
    {
        var office = await _officeSnapshotRepository.GetByIdAsync(doctor.OfficeId);

        return new DoctorResponseDto
        {
            Id = doctor.Id,
            PhotoUrl = doctor.PhotoUrl,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            MiddleName = doctor.MiddleName,
            DateOfBirth = doctor.DateOfBirth,
            Email = doctor.Email,
            Specialization = doctor.Specialization,
            OfficeId = doctor.OfficeId,
            OfficeAddress = office?.Address,
            CareerStartYear = doctor.CareerStartYear,
            Experience = DateTime.Today.Year - doctor.CareerStartYear + 1,
            Status = doctor.Status
        };
    }
}
