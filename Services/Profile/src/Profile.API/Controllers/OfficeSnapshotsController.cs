using Clinic.Profile.Application.DTOs.Offices;
using Clinic.Profile.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Profile.API.Controllers;

[ApiController]
[Route("api/office-snapshots")]
public class OfficeSnapshotsController : ControllerBase
{
    private readonly IOfficeSnapshotService _officeSnapshotService;

    public OfficeSnapshotsController(IOfficeSnapshotService officeSnapshotService)
    {
        _officeSnapshotService = officeSnapshotService;
    }

    // placeholder until async office sync is wired
    [HttpPost]
    public async Task<ActionResult<OfficeSnapshotResponseDto>> Upsert(
        UpsertOfficeSnapshotRequestDto request)
    {
        var result = await _officeSnapshotService.UpsertAsync(request);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OfficeSnapshotResponseDto>> GetById(Guid id)
    {
        var snapshot = await _officeSnapshotService.GetByIdAsync(id);

        if (snapshot is null)
        {
            return NotFound();
        }

        return Ok(snapshot);
    }
}
