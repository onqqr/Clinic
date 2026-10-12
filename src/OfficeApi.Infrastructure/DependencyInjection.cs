using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeApi.Application.Interfaces;
using OfficeApi.Infrastructure.Data;
using OfficeApi.Infrastructure.Repositories;

namespace OfficeApi.Infrastructure;

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