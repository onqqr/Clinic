using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Services;
using ProfilesApi.Application.Validators;

namespace ProfilesApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IDoctorService, DoctorService>();

        services.AddValidatorsFromAssemblyContaining<CreateDoctorRequestValidator>();

        return services;
    }
}
