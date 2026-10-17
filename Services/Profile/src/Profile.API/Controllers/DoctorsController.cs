using Clinic.Profile.Application.DTOs.Doctors;
using Clinic.Profile.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Profile.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IValidator<CreateDoctorRequestDto> _createValidator;
    private readonly IValidator<UpdateDoctorRequestDto> _updateValidator;
    private readonly IValidator<ChangeDoctorStatusRequestDto> _changeStatusValidator;

    public DoctorsController(
        IDoctorService doctorService,
        IValidator<CreateDoctorRequestDto> createValidator,
        IValidator<UpdateDoctorRequestDto> updateValidator,
        IValidator<ChangeDoctorStatusRequestDto> changeStatusValidator)
    {
        _doctorService = doctorService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _changeStatusValidator = changeStatusValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<DoctorResponseDto>>> GetAll(
        [FromQuery] DoctorFilterDto filter)
    {
        var doctors = await _doctorService.GetAllAsync(filter);

        return Ok(doctors);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DoctorResponseDto>> GetById(Guid id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);

        if (doctor is null)
        {
            return NotFound();
        }

        return Ok(doctor);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorResponseDto>> Create(
        CreateDoctorRequestDto request)
    {
        var validation = await _createValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var doctor = await _doctorService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = doctor.Id },
            doctor);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<DoctorResponseDto>> Update(
        Guid id,
        UpdateDoctorRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var doctor = await _doctorService.UpdateAsync(id, request);

        if (doctor is null)
        {
            return NotFound();
        }

        return Ok(doctor);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _doctorService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<DoctorResponseDto>> ChangeStatus(
        Guid id,
        ChangeDoctorStatusRequestDto request)
    {
        var validation = await _changeStatusValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var doctor = await _doctorService.ChangeStatusAsync(id, request);

        if (doctor is null)
        {
            return NotFound();
        }

        return Ok(doctor);
    }
}
