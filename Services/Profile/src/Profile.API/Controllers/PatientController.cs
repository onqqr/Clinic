using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Clinic.Profile.Application.DTOs.Patients;
using Clinic.Profile.Application.Interfaces;

namespace Clinic.Profile.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _service;
    private readonly IValidator<CreatePatientRequestDto> _createValidator;
    private readonly IValidator<UpdatePatientRequestDto> _updateValidator;
    private readonly IValidator<RegisterPatientRequestDto> _registerValidator;

    public PatientsController(
        IPatientService service,
        IValidator<CreatePatientRequestDto> createValidator,
        IValidator<UpdatePatientRequestDto> updateValidator,
        IValidator<RegisterPatientRequestDto> registerValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _registerValidator = registerValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<PatientResponseDto>>> GetAll(
        [FromQuery] string? fullName)
    {
        var patients = await _service.GetAllAsync(fullName);

        if (!string.IsNullOrWhiteSpace(fullName) && patients.Count == 0)
            return Ok(new { message = "No matches found", items = patients });

        return Ok(patients);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientResponseDto>> GetById(Guid id)
    {
        var patient = await _service.GetByIdAsync(id);
        return patient is null ? NotFound() : Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponseDto>> Create(
        CreatePatientRequestDto request)
    {
        var validation = await _createValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        var patient = await _service.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = patient.Id },
            patient);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PatientResponseDto>> Update(
        Guid id,
        UpdatePatientRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        var patient = await _service.UpdateAsync(id, request);
        return patient is null ? NotFound() : Ok(patient);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<PatientRegistrationResultDto>> Register(
        RegisterPatientRequestDto request)
    {
        var validation = await _registerValidator.ValidateAsync(request);
        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        var result = await _service.RegisterAsync(request);
        return Ok(result);
    }

    [HttpPost("confirm-match")]
    public async Task<ActionResult<PatientRegistrationResultDto>> ConfirmMatch(
        ConfirmPatientMatchRequestDto request)
    {
        var validation = await _registerValidator.ValidateAsync(request.RegistrationData);
        if (!validation.IsValid)
            return BadRequest(validation.Errors);

        try
        {
            var result = await _service.ConfirmMatchAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}