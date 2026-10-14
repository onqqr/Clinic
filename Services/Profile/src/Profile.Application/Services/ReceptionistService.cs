using Clinic.Profile.Application.DTOs.Receptionist;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Domain.Entities;

namespace Clinic.Profile.Application.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly IReceptionistRepository _repository;

    public ReceptionistService(IReceptionistRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ReceptionistResponseDto>> GetAllAsync(string? fullName = null)
    {
        var receptionists = await _repository.GetAllAsync(fullName);
        return receptionists.Select(MapToDto).ToList();
    }

    public async Task<ReceptionistResponseDto?> GetByIdAsync(Guid id)
    {
        var receptionist = await _repository.GetByIdAsync(id);
        return receptionist is null ? null : MapToDto(receptionist);
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
        return MapToDto(receptionist);
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
        return MapToDto(receptionist);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var receptionist = await _repository.GetByIdAsync(id);
        if (receptionist is null)
            return false;

        await _repository.DeleteAsync(receptionist);
        return true;
    }

    private static ReceptionistResponseDto MapToDto(Receptionist receptionist) => new()
    {
        Id = receptionist.Id,
        PhotoUrl = receptionist.PhotoUrl,
        FirstName = receptionist.FirstName,
        LastName = receptionist.LastName,
        MiddleName = receptionist.MiddleName,
        Email = receptionist.Email,
        OfficeId = receptionist.OfficeId
    };
}