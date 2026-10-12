using OfficeApi.Application.DTOs;
using OfficeApi.Application.Interfaces;
using OfficeApi.Domain.Entities;

namespace OfficeApi.Application.Services;

public class OfficeService : IOfficeService
{
    private readonly IOfficeRepository _officeRepository;

    public OfficeService(IOfficeRepository officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<List<OfficeResponseDto>> GetAllAsync()
    {
        var offices = await _officeRepository.GetAllAsync();

        return offices.Select(MapToResponse).ToList();
    }

    public async Task<OfficeResponseDto?> GetByIdAsync(Guid id)
    {
        var office = await _officeRepository.GetByIdAsync(id);

        if (office is null)
        {
            return null;
        }

        return MapToResponse(office);
    }

    public async Task<OfficeResponseDto> CreateAsync(CreateOfficeRequestDto request)
    {
        var office = new Office
        {
            Id = Guid.NewGuid(),
            PhotoUrl = request.PhotoUrl,
            City = request.City,
            Street = request.Street,
            HouseNumber = request.HouseNumber,
            OfficeNumber = request.OfficeNumber,
            RegistryPhoneNumber = request.RegistryPhoneNumber,
        };

        await _officeRepository.AddAsync(office);

        return MapToResponse(office);
    }

    public async Task<OfficeResponseDto?> UpdateAsync(Guid id, UpdateOfficeRequestDto request)
    {
        var office = await _officeRepository.GetByIdAsync(id);

        if (office is null)
        {
            return null;
        }

        office.PhotoUrl = request.PhotoUrl;
        office.City = request.City;
        office.Street = request.Street;
        office.HouseNumber = request.HouseNumber;
        office.OfficeNumber = request.OfficeNumber;
        office.RegistryPhoneNumber = request.RegistryPhoneNumber;

        await _officeRepository.UpdateAsync(office);

        return MapToResponse(office);
    }

    public async Task<OfficeResponseDto?> ChangeStatusAsync(
        Guid id,
        ChangeOfficeStatusRequestDto request)
    {
        var office = await _officeRepository.GetByIdAsync(id);

        if (office is null)
        {
            return null;
        }

        office.Status = request.Status;
        await _officeRepository.UpdateAsync(office);

        return MapToResponse(office);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var office = await _officeRepository.GetByIdAsync(id);

        if (office is null)
        {
            return false;
        }

        await _officeRepository.DeleteAsync(office);

        return true;
    }

    private static OfficeResponseDto MapToResponse(Office office)
    {
        return new OfficeResponseDto
        {
            Id = office.Id,
            PhotoUrl = office.PhotoUrl,
            Address = office.Address,
            Status = office.Status,
            RegistryPhoneNumber = office.RegistryPhoneNumber
        };
    }
}
