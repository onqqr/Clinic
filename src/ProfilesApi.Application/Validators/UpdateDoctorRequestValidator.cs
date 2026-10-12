using FluentValidation;
using ProfilesApi.Application.DTOs.Doctors;

namespace ProfilesApi.Application.Validators;

public class UpdateDoctorRequestValidator
    : AbstractValidator<UpdateDoctorRequestDto>
{
    public UpdateDoctorRequestValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Specialization).NotEmpty();
        RuleFor(x => x.OfficeId).NotEmpty();
        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Today);
        RuleFor(x => x.CareerStartYear)
            .GreaterThan(0)
            .LessThanOrEqualTo(DateTime.Today.Year);
        RuleFor(x => x.Status).IsInEnum();
    }
}