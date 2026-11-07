using Clinic.Services.Application.DTOs.Services;

namespace Clinic.Services.Application.Interfaces;

public interface IServiceItemService
{
    Task<ServiceResponseDto?> GetByIdAsync(Guid id);

    Task<ServiceResponseDto?> CreateAsync(
        Guid specializationId,
        CreateServiceRequestDto request);

    Task<ServiceResponseDto?> UpdateAsync(Guid id, UpdateServiceRequestDto request);

    Task<ServiceResponseDto?> ChangeStatusAsync(
        Guid id,
        ChangeServiceStatusRequestDto request);
}
