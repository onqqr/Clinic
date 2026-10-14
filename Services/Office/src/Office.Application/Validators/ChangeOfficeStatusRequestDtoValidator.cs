using FluentValidation;
using Clinic.Offices.Application.DTOs;

namespace Clinic.Offices.Application.Validators;

public class ChangeOfficeStatusRequestDtoValidator : AbstractValidator<ChangeOfficeStatusRequestDto>
{
    public ChangeOfficeStatusRequestDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("invalid office status");
    }
}
