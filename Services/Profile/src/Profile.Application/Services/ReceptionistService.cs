using Clinic.Profile.Application.DTOs.Receptionists;
using Clinic.Profile.Application.Events;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Interfaces;

namespace Clinic.Profile.Application.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly IReceptionistRepository _receptionistRepository;
    private readonly IOfficeSnapshotRepository _officeSnapshotRepository;
    private readonly IEventPublisher _eventPublisher;

    public ReceptionistService(
        IReceptionistRepository receptionistRepository,
        IOfficeSnapshotRepository officeSnapshotRepository,
        IEventPublisher eventPublisher)
    {
        _receptionistRepository = receptionistRepository;
        _officeSnapshotRepository = officeSnapshotRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<List<ReceptionistResponseDto>> GetAllAsync(string? fullName = null)
    {
        var receptionists = await _receptionistRepository.GetAllAsync(fullName);
        var result = new List<ReceptionistResponseDto>();

        foreach (var receptionist in receptionists)
        {
            result.Add(await MapToDtoAsync(receptionist));
        }

        return result;
    }

    public async Task<ReceptionistResponseDto?> GetByIdAsync(Guid id)
    {
        var receptionist = await _receptionistRepository.GetByIdAsync(id);

        if (receptionist is null)
        {
            return null;
        }

        return await MapToDtoAsync(receptionist);
    }

    public async Task<ReceptionistResponseDto> CreateAsync(
        CreateReceptionistRequestDto request)
    {
        if (await _receptionistRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException(
                "User with this email already exists.");
        }

        var receptionist = new Receptionist
        {
            PhotoUrl = request.PhotoUrl,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            Email = request.Email,
            OfficeId = request.OfficeId
        };

        await _receptionistRepository.AddAsync(receptionist);

        await _eventPublisher.PublishAsync(
            new ReceptionistProfileCreated(receptionist.Id, receptionist.Email));

        return await MapToDtoAsync(receptionist);
    }

    public async Task<ReceptionistResponseDto?> UpdateAsync(
        Guid id,
        UpdateReceptionistRequestDto request)
    {
        var receptionist = await _receptionistRepository.GetByIdAsync(id);

        if (receptionist is null)
        {
            return null;
        }

        if (await _receptionistRepository.EmailExistsAsync(request.Email, id))
        {
            throw new InvalidOperationException(
                "User with this email already exists.");
        }

        receptionist.PhotoUrl = request.PhotoUrl;
        receptionist.FirstName = request.FirstName;
        receptionist.LastName = request.LastName;
        receptionist.MiddleName = request.MiddleName;
        receptionist.Email = request.Email;
        receptionist.OfficeId = request.OfficeId;

        await _receptionistRepository.UpdateAsync(receptionist);

        return await MapToDtoAsync(receptionist);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var receptionist = await _receptionistRepository.GetByIdAsync(id);

        if (receptionist is null)
        {
            return false;
        }

        await _receptionistRepository.DeleteAsync(receptionist);

        return true;
    }

    private async Task<ReceptionistResponseDto> MapToDtoAsync(Receptionist receptionist)
    {
        var office = await _officeSnapshotRepository.GetByIdAsync(receptionist.OfficeId);

        return new ReceptionistResponseDto
        {
            Id = receptionist.Id,
            PhotoUrl = receptionist.PhotoUrl,
            FirstName = receptionist.FirstName,
            LastName = receptionist.LastName,
            MiddleName = receptionist.MiddleName,
            Email = receptionist.Email,
            OfficeId = receptionist.OfficeId,
            OfficeAddress = office?.Address
        };
    }
}
