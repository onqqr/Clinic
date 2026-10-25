using Clinic.Services.Application.DTOs.Services;
using Clinic.Services.Application.DTOs.Specializations;
using Clinic.Services.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Services.API.Controllers;

[ApiController]
[Route("api/specializations")]
public class SpecializationsController : ControllerBase
{
    private readonly ISpecializationService _specializationService;
    private readonly IServiceItemService _serviceItemService;
    private readonly IValidator<CreateSpecializationRequestDto> _createValidator;
    private readonly IValidator<UpdateSpecializationRequestDto> _updateValidator;
    private readonly IValidator<ChangeSpecializationStatusRequestDto> _changeStatusValidator;
    private readonly IValidator<CreateServiceRequestDto> _createServiceValidator;

    public SpecializationsController(
        ISpecializationService specializationService,
        IServiceItemService serviceItemService,
        IValidator<CreateSpecializationRequestDto> createValidator,
        IValidator<UpdateSpecializationRequestDto> updateValidator,
        IValidator<ChangeSpecializationStatusRequestDto> changeStatusValidator,
        IValidator<CreateServiceRequestDto> createServiceValidator)
    {
        _specializationService = specializationService;
        _serviceItemService = serviceItemService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _changeStatusValidator = changeStatusValidator;
        _createServiceValidator = createServiceValidator;
    }

    [HttpGet]
    public async Task<ActionResult<List<SpecializationListItemDto>>> GetAll()
    {
        var items = await _specializationService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SpecializationResponseDto>> GetById(Guid id)
    {
        var item = await _specializationService.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<SpecializationResponseDto>> Create(
        CreateSpecializationRequestDto request)
    {
        var validation = await _createValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var created = await _specializationService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SpecializationResponseDto>> Update(
        Guid id,
        UpdateSpecializationRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var updated = await _specializationService.UpdateAsync(id, request);

        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<SpecializationResponseDto>> ChangeStatus(
        Guid id,
        ChangeSpecializationStatusRequestDto request)
    {
        var validation = await _changeStatusValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var updated = await _specializationService.ChangeStatusAsync(id, request);

        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpPost("{id:guid}/services")]
    public async Task<ActionResult<ServiceResponseDto>> CreateService(
        Guid id,
        CreateServiceRequestDto request)
    {
        var validation = await _createServiceValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var created = await _serviceItemService.CreateAsync(id, request);

        if (created is null)
        {
            return NotFound();
        }

        return Created($"/api/services/{created.Id}", created);
    }
}
