using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OfficeApi.Application.Interfaces;
using OfficeApi.Application.Services;
using OfficeApi.Application.Validators;

namespace OfficeApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateOfficeRequestDtoValidator>();
        services.AddScoped<IOfficeService, OfficeService>();

        return services;
    }
}
