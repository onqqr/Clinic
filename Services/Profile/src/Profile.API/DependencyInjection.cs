using Clinic.Profile.Application.Interfaces;
using Clinic.Profile.Application.Mapping;
using Clinic.Profile.Application.Services;
using Clinic.Profile.Application.Validators;
using Clinic.Profile.Domain.Interfaces;
using Clinic.Profile.Infrastructure.Data;
using Clinic.Profile.Infrastructure.Messaging;
using Clinic.Profile.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Profile.API;

public static class DependencyInjection
{
    public static IServiceCollection AddProfileServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(
            _ => { },
            typeof(DoctorMappingProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<CreateDoctorRequestValidator>();

        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IReceptionistService, ReceptionistService>();
        services.AddScoped<IOfficeSnapshotService, OfficeSnapshotService>();

        var connectionString =
            configuration.GetConnectionString("ProfilesDb")
            ?? throw new InvalidOperationException(
                "Connection string 'ProfilesDb' was not found.");

        services.AddDbContext<ProfilesDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IReceptionistRepository, ReceptionistRepository>();
        services.AddScoped<IOfficeSnapshotRepository, OfficeSnapshotRepository>();
        services.AddScoped<IEventPublisher, LoggingEventPublisher>();

        return services;
    }
}
