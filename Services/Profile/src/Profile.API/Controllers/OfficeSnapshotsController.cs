using Microsoft.AspNetCore.Mvc;
using Clinic.Profile.Application.DTOs.Offices;
using Clinic.Profile.Application.Interfaces;

namespace Clinic.Profile.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OfficeSnapshotsController : ControllerBase
{
    private readonly IOfficeSnapshotService _service;

    public OfficeSnapshotsController(IOfficeSnapshotService service)
    {
        _service = service;
    }
    
    // placeholder test (rabbitMQ will come later)
    [HttpPost]
    public async Task<ActionResult<OfficeSnapshotResponseDto>> Upsert(
        UpsertOfficeSnapshotRequestDto request)
    {
        var result = await _service.UpsertAsync(request);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OfficeSnapshotResponseDto>> GetById(Guid id)
    {
        var snapshot = await _service.GetByIdAsync(id);
        return snapshot is null ? NotFound() : Ok(snapshot);
    }
}