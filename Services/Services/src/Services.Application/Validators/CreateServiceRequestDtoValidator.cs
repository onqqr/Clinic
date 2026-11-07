using Clinic.Services.Application.DTOs.Services;
using FluentValidation;

namespace Clinic.Services.Application.Validators;

public class CreateServiceRequestDtoValidator : AbstractValidator<CreateServiceRequestDto>
{
    public CreateServiceRequestDtoValidator()
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
