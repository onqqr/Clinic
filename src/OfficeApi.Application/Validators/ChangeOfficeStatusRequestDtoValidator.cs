using FluentValidation;
using OfficeApi.Application.DTOs;

namespace OfficeApi.Application.Validators;

public class ChangeOfficeStatusRequestDtoValidator : AbstractValidator<ChangeOfficeStatusRequestDto>
{
    public ChangeOfficeStatusRequestDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("invalid office status");
    }
}
