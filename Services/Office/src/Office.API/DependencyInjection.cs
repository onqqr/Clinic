using Clinic.Offices.Application.Interfaces;
using Clinic.Offices.Application.Services;
using Clinic.Offices.Application.Validators;
using Clinic.Offices.Infrastructure.Data;
using Clinic.Offices.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Offices.API;

public static class DependencyInjection
{
    public static IServiceCollection AddOfficeServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<CreateOfficeRequestDtoValidator>();
        services.AddScoped<IOfficeService, OfficeService>();

        services.AddDbContext<OfficeDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("OfficeDb")));
        services.AddScoped<IOfficeRepository, OfficeRepository>();

        return services;
    }
}
