using Clinic.Services.Application.DTOs.Specializations;
using FluentValidation;

namespace Clinic.Services.Application.Validators;

public class CreateSpecializationRequestDtoValidator
    : AbstractValidator<CreateSpecializationRequestDto>
{
    public CreateSpecializationRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Please, enter the specialization name");

        RuleFor(x => x.Status)
            .IsInEnum();

        RuleFor(x => x.Services)
            .NotEmpty()
            .WithMessage("Specialization must contain at least one service");

        RuleForEach(x => x.Services)
            .SetValidator(new CreateServiceRequestDtoValidator());
    }
}
