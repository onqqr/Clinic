using Clinic.Offices.Application.DTOs;

namespace Clinic.Offices.Application.Interfaces;

public interface IOfficeService
{
    Task<List<OfficeResponseDto>> GetAllAsync();

    Task<OfficeResponseDto?> GetByIdAsync(Guid id);

    Task<OfficeResponseDto> CreateAsync(CreateOfficeRequestDto request);

    Task<OfficeResponseDto?> UpdateAsync(Guid id, UpdateOfficeRequestDto request);

    Task<OfficeResponseDto?> ChangeStatusAsync(Guid id, ChangeOfficeStatusRequestDto request);

    Task<bool> DeleteAsync(Guid id);
}
