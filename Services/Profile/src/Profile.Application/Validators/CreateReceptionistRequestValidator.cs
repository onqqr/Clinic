using Clinic.Profile.Application.DTOs.Receptionist;
using FluentValidation;

namespace Clinic.Profile.Application.Validators;

public class CreateReceptionistRequestValidator
    : AbstractValidator<CreateReceptionistRequestDto>
{
    public CreateReceptionistRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.OfficeId)
            .NotEmpty();
    }
}