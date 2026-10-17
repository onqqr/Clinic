using Clinic.Profile.Application.DTOs.Patients;
using FluentValidation;

namespace Clinic.Profile.Application.Validators;

public class CreatePatientRequestValidator
    : AbstractValidator<CreatePatientRequestDto>
{
    public CreatePatientRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
    }
}
