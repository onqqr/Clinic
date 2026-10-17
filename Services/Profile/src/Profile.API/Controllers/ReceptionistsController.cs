using Clinic.Profile.Application.DTOs.Receptionists;
using Clinic.Profile.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Profile.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceptionistsController : ControllerBase
{
    private readonly IReceptionistService _receptionistService;
    private readonly IValidator<CreateReceptionistRequestDto> _createValidator;
    private readonly IValidator<UpdateReceptionistRequestDto> _updateValidator;

    public ReceptionistsController(
        IReceptionistService receptionistService,
        IValidator<CreateReceptionistRequestDto> createValidator,
        IValidator<UpdateReceptionistRequestDto> updateValidator)
    {
        _receptionistService = receptionistService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReceptionistResponseDto>>> GetAll(
        [FromQuery] ReceptionistFilterDto filter)
    {
        var receptionists = await _receptionistService.GetAllAsync(filter);

        return Ok(receptionists);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReceptionistResponseDto>> GetById(Guid id)
    {
        var receptionist = await _receptionistService.GetByIdAsync(id);

        if (receptionist is null)
        {
            return NotFound();
        }

        return Ok(receptionist);
    }

    [HttpPost]
    public async Task<ActionResult<ReceptionistResponseDto>> Create(
        CreateReceptionistRequestDto request)
    {
        var validation = await _createValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var receptionist = await _receptionistService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = receptionist.Id },
            receptionist);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ReceptionistResponseDto>> Update(
        Guid id,
        UpdateReceptionistRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var receptionist = await _receptionistService.UpdateAsync(id, request);

        if (receptionist is null)
        {
            return NotFound();
        }

        return Ok(receptionist);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _receptionistService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
