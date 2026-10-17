using Clinic.Profile.Application.DTOs.Receptionists;

namespace Clinic.Profile.Application.Interfaces;

public interface IReceptionistService
{
    Task<List<ReceptionistResponseDto>> GetAllAsync(string? fullName = null);

    Task<ReceptionistResponseDto?> GetByIdAsync(Guid id);

    Task<ReceptionistResponseDto> CreateAsync(CreateReceptionistRequestDto request);

    Task<ReceptionistResponseDto?> UpdateAsync(Guid id, UpdateReceptionistRequestDto request);

    Task<bool> DeleteAsync(Guid id);
}
