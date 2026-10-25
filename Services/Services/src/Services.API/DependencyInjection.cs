using Clinic.Services.Application.Interfaces;
using Clinic.Services.Application.Services;
using Clinic.Services.Application.Validators;
using Clinic.Services.Domain.Interfaces;
using Clinic.Services.Infrastructure.Data;
using Clinic.Services.Infrastructure.Messaging;
using Clinic.Services.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Services.API;

public static class DependencyInjection
{
    public static IServiceCollection AddServicesModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<CreateSpecializationRequestDtoValidator>();

        services.AddScoped<ISpecializationService, SpecializationService>();
        services.AddScoped<IServiceItemService, ServiceItemService>();

        var connectionString =
            configuration.GetConnectionString("ServicesDb")
            ?? throw new InvalidOperationException(
                "Connection string 'ServicesDb' was not found.");

        services.AddDbContext<ServicesDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ISpecializationRepository, SpecializationRepository>();
        services.AddScoped<IServiceItemRepository, ServiceItemRepository>();
        services.AddScoped<IEventPublisher, LoggingEventPublisher>();

        return services;
    }
}
