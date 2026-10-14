
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Clinic.Profile.Application.DTOs.Receptionist;
using Clinic.Profile.Application.Interfaces;

namespace Clinic.Profile.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceptionistsController : ControllerBase
{
    private readonly IReceptionistService _service;
    private readonly IValidator<CreateReceptionistRequestDto> _createValidator;
    private readonly IValidator<UpdateReceptionistRequestDto> _updateValidator;

    public ReceptionistsController(
        IReceptionistService service,
        IValidator<CreateReceptionistRequestDto> createValidator,
        IValidator<UpdateReceptionistRequestDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReceptionistResponseDto>>> GetAll(
        [FromQuery] string? fullName)
    {
        var receptionists = await _service.GetAllAsync(
            fullName);

        return Ok(receptionists);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReceptionistResponseDto>> GetById(Guid id)
    {
        var receptionist = await _service.GetByIdAsync(id);

        return receptionist is null ? NotFound() : Ok(receptionist);
    }

    [HttpPost]
    public async Task<ActionResult<ReceptionistResponseDto>> Create(
        CreateReceptionistRequestDto request)
    {
        var validation = await _createValidator.ValidateAsync(request);

        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        try
        {
            var receptionist = await _service.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = receptionist.Id },
                receptionist);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ReceptionistResponseDto>> Update(
        Guid id,
        UpdateReceptionistRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);

        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        try
        {
            var receptionist = await _service.UpdateAsync(id, request);

            return receptionist is null ? NotFound() : Ok(receptionist);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);

        return deleted ? NoContent() : NotFound();
    }
}
