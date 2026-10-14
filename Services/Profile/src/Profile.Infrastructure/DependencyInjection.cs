namespace Clinic.Profile.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Clinic.Profile.Infrastructure.Data;
using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Infrastructure.Repositories;
using Clinic.Profile.Application.Services;

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
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IReceptionistRepository, ReceptionistRepository>();

        return services;
    }
}