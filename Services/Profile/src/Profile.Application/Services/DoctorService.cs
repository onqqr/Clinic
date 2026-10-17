using AutoMapper;
using Clinic.Profile.Application.DTOs.Doctors;
using Clinic.Profile.Application.Events;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Interfaces;
using Clinic.Profile.Domain.Specifications;

namespace Clinic.Profile.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IOfficeSnapshotRepository _officeSnapshotRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IMapper _mapper;

    public DoctorService(
        IDoctorRepository doctorRepository,
        IOfficeSnapshotRepository officeSnapshotRepository,
        IEventPublisher eventPublisher,
        IMapper mapper)
    {
        _doctorRepository = doctorRepository;
        _officeSnapshotRepository = officeSnapshotRepository;
        _eventPublisher = eventPublisher;
        _mapper = mapper;
    }

    public async Task<List<DoctorResponseDto>> GetAllAsync(DoctorFilterDto filter)
    {
        var specification = new DoctorFilterSpecification(
            filter.FullName,
            filter.Specialization,
            filter.OfficeId,
            filter.OnlyAvailable);

        var doctors = await _doctorRepository.ListAsync(specification);
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

    public async Task<DoctorResponseDto> CreateAsync(CreateDoctorRequestDto request)
    {
        if (await _doctorRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException(
                "User with this email already exists.");
        }

        var doctor = _mapper.Map<Doctor>(request);
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

        _mapper.Map(request, doctor);
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
        var dto = _mapper.Map<DoctorResponseDto>(doctor);
        var office = await _officeSnapshotRepository.GetByIdAsync(doctor.OfficeId);

        return dto with { OfficeAddress = office?.Address };
    }
}
