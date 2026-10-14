using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Clinic.Offices.Application.Interfaces;
using Clinic.Offices.Infrastructure.Data;
using Clinic.Offices.Infrastructure.Repositories;

namespace Clinic.Offices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)

    {
        services.AddDbContext<OfficeDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("OfficeDb")));

        services.AddScoped<IOfficeRepository, OfficeRepository>();
        return services;
    }
}