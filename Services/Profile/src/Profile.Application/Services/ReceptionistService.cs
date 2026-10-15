using Clinic.Profile.Application.DTOs.Receptionist;
using Clinic.Profile.Application.Events;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly IReceptionistRepository _repository;
    private readonly IOfficeSnapshotRepository  _officeSnapshot;
    private readonly IEventPublisher _eventPublisher;

    public ReceptionistService(
        IReceptionistRepository repository, 
        IOfficeSnapshotRepository officeSnapshot,
        IEventPublisher eventPublisher)
    {
        _repository = repository;
        _officeSnapshot = officeSnapshot;
        _eventPublisher = eventPublisher;
    }

    public async Task<List<ReceptionistResponseDto>> GetAllAsync(string? fullName = null)
    {
        var receptionists = await _repository.GetAllAsync(fullName);
        var result = new List<ReceptionistResponseDto>();
        foreach (var receptionist in receptionists)
        {
            result.Add(await MapToDtoAsync(receptionist));
        }
        return result;
    }

    public async Task<ReceptionistResponseDto?> GetByIdAsync(Guid id)
    {
        var receptionist = await _repository.GetByIdAsync(id);
        return receptionist is null ? null : await MapToDtoAsync(receptionist);
    }

    public async Task<ReceptionistResponseDto> CreateAsync(
        CreateReceptionistRequestDto request)
    {
        if (await _repository.EmailExistsAsync(request.Email))
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

        await _repository.AddAsync(receptionist);
        
        await _eventPublisher.PublishAsync(
            new ReceptionistProfileCreated(receptionist.Id, receptionist.Email));
        
        return await MapToDtoAsync(receptionist);
    }

    public async Task<ReceptionistResponseDto?> UpdateAsync(
        Guid id,
        UpdateReceptionistRequestDto request)
    {
        var receptionist = await _repository.GetByIdAsync(id);
        if (receptionist is null)
            return null;

        if (await _repository.EmailExistsAsync(request.Email, id))
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

        await _repository.UpdateAsync(receptionist);
        return await MapToDtoAsync(receptionist);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var receptionist = await _repository.GetByIdAsync(id);
        if (receptionist is null)
            return false;

        await _repository.DeleteAsync(receptionist);
        return true;
    }

    private async Task<ReceptionistResponseDto> MapToDtoAsync(Receptionist receptionist)
    {
        var office = await _officeSnapshot.GetByIdAsync(receptionist.OfficeId);

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