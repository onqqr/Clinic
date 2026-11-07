using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Clinic.Offices.Application.DTOs;
using Clinic.Offices.Application.Interfaces;

namespace Clinic.Offices.API.Controllers;

[ApiController]
[Route("api/offices")]
public class OfficesController : ControllerBase
{
    private readonly IOfficeService _officeService;
    private readonly IValidator<CreateOfficeRequestDto> _createValidator;
    private readonly IValidator<UpdateOfficeRequestDto> _updateValidator;
    private readonly IValidator<ChangeOfficeStatusRequestDto> _changeStatusValidator;

    public OfficesController(
        IOfficeService officeService,
        IValidator<CreateOfficeRequestDto> createValidator,
        IValidator<UpdateOfficeRequestDto> updateValidator,
        IValidator<ChangeOfficeStatusRequestDto> changeStatusValidator)
    {
        _officeService = officeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _changeStatusValidator = changeStatusValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<OfficeResponseDto>>> GetAll()
    {
        var offices = await _officeService.GetAllAsync();

        return Ok(offices);
    }

    [HttpPost]
    public async Task<ActionResult<OfficeResponseDto>> Create(CreateOfficeRequestDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var office = await _officeService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = office.Id },
            office);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OfficeResponseDto>> GetById(Guid id)
    {
        var office = await _officeService.GetByIdAsync(id);

        if (office is null)
        {
            return NotFound();
        }

        return Ok(office);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<OfficeResponseDto>> Update(
        Guid id,
        UpdateOfficeRequestDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var office = await _officeService.UpdateAsync(id, request);

        if (office is null)
        {
            return NotFound();
        }

        return Ok(office);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<OfficeResponseDto>> ChangeStatus(
        Guid id,
        ChangeOfficeStatusRequestDto request)
    {
        var validationResult = await _changeStatusValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var office = await _officeService.ChangeStatusAsync(id, request);

        if (office is null)
        {
            return NotFound();
        }

        return Ok(office);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _officeService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
