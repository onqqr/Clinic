using AutoMapper;
using Clinic.Profile.Application.DTOs.Receptionists;
using Clinic.Profile.Application.Events;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;
using Clinic.Profile.Domain.Interfaces;
using Clinic.Profile.Domain.Specifications;

namespace Clinic.Profile.Application.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly IReceptionistRepository _receptionistRepository;
    private readonly IOfficeSnapshotRepository _officeSnapshotRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IMapper _mapper;

    public ReceptionistService(
        IReceptionistRepository receptionistRepository,
        IOfficeSnapshotRepository officeSnapshotRepository,
        IEventPublisher eventPublisher,
        IMapper mapper)
    {
        _receptionistRepository = receptionistRepository;
        _officeSnapshotRepository = officeSnapshotRepository;
        _eventPublisher = eventPublisher;
        _mapper = mapper;
    }

    public async Task<List<ReceptionistResponseDto>> GetAllAsync(ReceptionistFilterDto filter)
    {
        var specification = new ReceptionistFilterSpecification(filter.FullName);
        var receptionists = await _receptionistRepository.ListAsync(specification);
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

        var receptionist = _mapper.Map<Receptionist>(request);
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

        _mapper.Map(request, receptionist);
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
        var dto = _mapper.Map<ReceptionistResponseDto>(receptionist);
        var office = await _officeSnapshotRepository.GetByIdAsync(receptionist.OfficeId);

        return dto with { OfficeAddress = office?.Address };
    }
}
