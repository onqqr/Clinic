using Clinic.Profile.Application.DTOs.Patients;
using Clinic.Profile.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Profile.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IValidator<CreatePatientRequestDto> _createValidator;
    private readonly IValidator<UpdatePatientRequestDto> _updateValidator;
    private readonly IValidator<RegisterPatientRequestDto> _registerValidator;

    public PatientsController(
        IPatientService patientService,
        IValidator<CreatePatientRequestDto> createValidator,
        IValidator<UpdatePatientRequestDto> updateValidator,
        IValidator<RegisterPatientRequestDto> registerValidator)
    {
        _patientService = patientService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _registerValidator = registerValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<PatientResponseDto>>> GetAll(
        [FromQuery] PatientFilterDto filter)
    {
        var patients = await _patientService.GetAllAsync(filter);

        if (!string.IsNullOrWhiteSpace(filter.FullName) && patients.Count == 0)
        {
            return Ok(new { message = "No matches found", items = patients });
        }

        return Ok(patients);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientResponseDto>> GetById(Guid id)
    {
        var patient = await _patientService.GetByIdAsync(id);

        if (patient is null)
        {
            return NotFound();
        }

        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponseDto>> Create(
        CreatePatientRequestDto request)
    {
        var validation = await _createValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var patient = await _patientService.CreateAsync(request);

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
        {
            return BadRequest(validation.Errors);
        }

        var patient = await _patientService.UpdateAsync(id, request);

        if (patient is null)
        {
            return NotFound();
        }

        return Ok(patient);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _patientService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("register")]
    public async Task<ActionResult<PatientRegistrationResultDto>> Register(
        RegisterPatientRequestDto request)
    {
        var validation = await _registerValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var result = await _patientService.RegisterAsync(request);

        return Ok(result);
    }

    [HttpPost("confirm-match")]
    public async Task<ActionResult<PatientRegistrationResultDto>> ConfirmMatch(
        ConfirmPatientMatchRequestDto request)
    {
        var validation = await _registerValidator.ValidateAsync(request.RegistrationData);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var result = await _patientService.ConfirmMatchAsync(request);

        return Ok(result);
    }
}
