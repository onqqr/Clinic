using Clinic.Services.Application.DTOs.Services;
using Clinic.Services.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Services.API.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly IServiceItemService _serviceItemService;
    private readonly IValidator<UpdateServiceRequestDto> _updateValidator;
    private readonly IValidator<ChangeServiceStatusRequestDto> _changeStatusValidator;

    public ServicesController(
        IServiceItemService serviceItemService,
        IValidator<UpdateServiceRequestDto> updateValidator,
        IValidator<ChangeServiceStatusRequestDto> changeStatusValidator)
    {
        _serviceItemService = serviceItemService;
        _updateValidator = updateValidator;
        _changeStatusValidator = changeStatusValidator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceResponseDto>> GetById(Guid id)
    {
        var service = await _serviceItemService.GetByIdAsync(id);

        if (service is null)
        {
            return NotFound();
        }

        return Ok(service);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ServiceResponseDto>> Update(
        Guid id,
        UpdateServiceRequestDto request)
    {
        var validation = await _updateValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var updated = await _serviceItemService.UpdateAsync(id, request);

        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<ServiceResponseDto>> ChangeStatus(
        Guid id,
        ChangeServiceStatusRequestDto request)
    {
        var validation = await _changeStatusValidator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors);
        }

        var updated = await _serviceItemService.ChangeStatusAsync(id, request);

        if (updated is null)
        {
            return NotFound();
        }

        return Ok(updated);
    }
}
