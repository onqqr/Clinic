using Clinic.Profile.Application.DTOs.Doctors;
using FluentValidation;

namespace Clinic.Profile.Application.Validators;

public class ChangeDoctorStatusRequestValidator
    : AbstractValidator<ChangeDoctorStatusRequestDto>
{
    public ChangeDoctorStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}