using Clinic.Services.Application.DTOs.Services;
using FluentValidation;

namespace Clinic.Services.Application.Validators;

public class ChangeServiceStatusRequestDtoValidator
    : AbstractValidator<ChangeServiceStatusRequestDto>
{
    public ChangeServiceStatusRequestDtoValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}
