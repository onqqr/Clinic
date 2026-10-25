using Clinic.Services.Application.DTOs.Specializations;
using FluentValidation;

namespace Clinic.Services.Application.Validators;

public class ChangeSpecializationStatusRequestDtoValidator
    : AbstractValidator<ChangeSpecializationStatusRequestDto>
{
    public ChangeSpecializationStatusRequestDtoValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}
