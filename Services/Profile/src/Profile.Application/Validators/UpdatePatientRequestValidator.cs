using Clinic.Profile.Application.DTOs.Patients;
using FluentValidation;

namespace Clinic.Profile.Application.Validators;

public class UpdatePatientRequestValidator
    : AbstractValidator<UpdatePatientRequestDto>
{
    public UpdatePatientRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
    }
}