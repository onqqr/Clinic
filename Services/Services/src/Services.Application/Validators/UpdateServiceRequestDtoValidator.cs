using Clinic.Services.Application.DTOs.Services;
using FluentValidation;

namespace Clinic.Services.Application.Validators;

public class UpdateServiceRequestDtoValidator : AbstractValidator<UpdateServiceRequestDto>
{
    public UpdateServiceRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Please, enter the service name");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("You've entered an invalid price");

        RuleFor(x => x.Category)
            .IsInEnum();

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
