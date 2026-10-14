using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Application.Services;
using Clinic.Profile.Application.Validators;

namespace Clinic.Profile.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IReceptionistService, ReceptionistService>();

        services.AddValidatorsFromAssemblyContaining<CreateDoctorRequestValidator>();

        return services;
    }
}
