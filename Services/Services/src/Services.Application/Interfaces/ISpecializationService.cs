using Clinic.Services.Application.DTOs.Catalog;
using Clinic.Services.Application.DTOs.Specializations;

namespace Clinic.Services.Application.Interfaces;

public interface ISpecializationService
{
    Task<List<SpecializationListItemDto>> GetAllAsync();

    Task<SpecializationResponseDto?> GetByIdAsync(Guid id);

    Task<SpecializationResponseDto> CreateAsync(CreateSpecializationRequestDto request);

    Task<SpecializationResponseDto?> UpdateAsync(Guid id, UpdateSpecializationRequestDto request);

    Task<SpecializationResponseDto?> ChangeStatusAsync(
        Guid id,
        ChangeSpecializationStatusRequestDto request);

    Task<PatientCatalogResponseDto> GetPatientCatalogAsync();
}
