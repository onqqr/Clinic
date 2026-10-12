using ProfilesApi.Application.DTOs.Doctors;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Services;

public class DoctorService :  IDoctorService
{
    private readonly IDoctorRepository _repository;

    public DoctorService(IDoctorRepository repository)
    {
        _repository = repository;
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

        return doctors.Select(MapToDto).ToList();
    }

    public async Task<DoctorResponseDto?> GetByIdAsync(Guid id)
    {
        var doctor = await _repository.GetByIdAsync(id);

        return doctor is null ? null : MapToDto(doctor);
    }

    public async Task<DoctorResponseDto> CreateAsync(
        CreateDoctorRequestDto request)
    {
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

        return MapToDto(doctor);
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

        return MapToDto(doctor);
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

    private static DoctorResponseDto MapToDto(Doctor doctor)
    {
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
            CareerStartYear = doctor.CareerStartYear,
            Experience = DateTime.Today.Year - doctor.CareerStartYear + 1,
            Status = doctor.Status
        };
    }
}