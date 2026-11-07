using Clinic.Services.Application.DTOs.Catalog;
using Clinic.Services.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Services.API.Controllers;

[ApiController]
[Route("api/catalog")]
public class CatalogController : ControllerBase
{
    private readonly ISpecializationService _specializationService;

    public CatalogController(ISpecializationService specializationService)
    {
        _specializationService = specializationService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PatientCatalogResponseDto>> GetPatientCatalog()
    {
        var catalog = await _specializationService.GetPatientCatalogAsync();
        return Ok(catalog);
    }
}
