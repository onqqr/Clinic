using Clinic.Profile.Application.DTOs.Doctors;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Services;

public class DoctorService :  IDoctorService
{
    private readonly IDoctorRepository _repository;
    private readonly IOfficeSnapshotRepository _officeSnapshots;

    public DoctorService(
        IDoctorRepository repository,
        IOfficeSnapshotRepository officeSnapshots)
    {
        _repository = repository;
        _officeSnapshots = officeSnapshots;
    }

    public async Task<List<DoctorResponseDto>> GetAllAsync(
        string? fullName = null,
        string? specialization = null,
        Guid? officeId = null,
        bool onlyAvailable = false)
    {
        var doctors = await _repository.GetAllAsync(
            fullName,
            specialization,
            officeId,
            onlyAvailable);
        
        var result = new List<DoctorResponseDto>();
        foreach (var doctor in doctors)
            result.Add(await MapToDtoAsync(doctor));

        return result;
    }

    public async Task<DoctorResponseDto?> GetByIdAsync(Guid id)
    {
        var doctor = await _repository.GetByIdAsync(id);

        return doctor is null ? null : await MapToDtoAsync(doctor);
    }

    public async Task<DoctorResponseDto> CreateAsync(
        CreateDoctorRequestDto request)
    {
        if (await _repository.EmailExistsAsync(request.Email))
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

        await _repository.AddAsync(doctor);

        return await MapToDtoAsync(doctor);
    }

    public async Task<DoctorResponseDto?> UpdateAsync(
        Guid id,
        UpdateDoctorRequestDto request)
    {
        var doctor = await _repository.GetByIdAsync(id);

        if (doctor is null)
        {
            return null;
        }
        
        if (await _repository.EmailExistsAsync(request.Email, id))
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

        await _repository.UpdateAsync(doctor);

        return await MapToDtoAsync(doctor);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var doctor = await _repository.GetByIdAsync(id);

        if (doctor is null)
        {
            return false;
        }

        await _repository.DeleteAsync(doctor);

        return true;
    }

    private async Task<DoctorResponseDto> MapToDtoAsync(Doctor doctor)
    {
        var office = await _officeSnapshots.GetByIdAsync(doctor.OfficeId);
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