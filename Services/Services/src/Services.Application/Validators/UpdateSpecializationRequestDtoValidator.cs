using Clinic.Services.Application.DTOs.Specializations;
using FluentValidation;

namespace Clinic.Services.Application.Validators;

public class UpdateSpecializationRequestDtoValidator
    : AbstractValidator<UpdateSpecializationRequestDto>
{
    public UpdateSpecializationRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Please, enter the specialization name");

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
