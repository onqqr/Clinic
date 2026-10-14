using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Clinic.Profile.Application.DTOs.Doctors;
using Clinic.Profile.Application.Interfaces;

namespace Clinic.Profile.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _service;
    private readonly IValidator<CreateDoctorRequestDto> _createValidator;
    private readonly IValidator<UpdateDoctorRequestDto> _updateValidator;

    public DoctorsController(
        IDoctorService service,
        IValidator<CreateDoctorRequestDto> createValidator,
        IValidator<UpdateDoctorRequestDto> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<DoctorResponseDto>>> GetAll(
        [FromQuery] string? fullName,
        [FromQuery] string? specialization,
        [FromQuery] Guid? officeId,
        [FromQuery] bool onlyAvailable = false)
    {
        var doctors = await _service.GetAllAsync(
            fullName, specialization, officeId, onlyAvailable);

        return Ok(doctors);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DoctorResponseDto>> GetById(Guid id)
    {
        var doctor = await _service.GetByIdAsync(id);

        return doctor is null ? NotFound() : Ok(doctor);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorResponseDto>> Create(
        CreateDoctorRequestDto request)
    {
        var validation = await _createValidator.ValidateAsync(request);

        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        try
        {
            var doctor = await _service.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = doctor.Id },
                doctor);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DoctorResponseDto>> Update(
        Guid id,
        UpdateDoctorRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);

        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        try
        {
            var doctor = await _service.UpdateAsync(id, request);

            return doctor is null ? NotFound() : Ok(doctor);
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
