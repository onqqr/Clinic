namespace ProfilesApi.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfilesApi.Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("ProfilesDb")
            ?? throw new InvalidOperationException(
                "Connection string 'ProfilesDb' was not found.");

        services.AddDbContext<ProfilesDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}