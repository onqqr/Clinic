using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Clinic.Offices.Application.Interfaces;
using Clinic.Offices.Application.Services;
using Clinic.Offices.Application.Validators;

namespace Clinic.Offices.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateOfficeRequestDtoValidator>();
        services.AddScoped<IOfficeService, OfficeService>();

        return services;
    }
}
