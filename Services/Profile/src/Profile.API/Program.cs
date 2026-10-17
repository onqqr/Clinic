using System.Text.Json.Serialization;
using Clinic.Profile.API.Middleware;
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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(
    _ => { },
    typeof(DoctorMappingProfile).Assembly);
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IReceptionistService, ReceptionistService>();
builder.Services.AddScoped<IOfficeSnapshotService, OfficeSnapshotService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDoctorRequestValidator>();

var connectionString =
    builder.Configuration.GetConnectionString("ProfilesDb")
    ?? throw new InvalidOperationException(
        "Connection string 'ProfilesDb' was not found.");

builder.Services.AddDbContext<ProfilesDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IReceptionistRepository, ReceptionistRepository>();
builder.Services.AddScoped<IOfficeSnapshotRepository, OfficeSnapshotRepository>();
builder.Services.AddScoped<IEventPublisher, LoggingEventPublisher>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ProfilesDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.MapControllers();

app.Run();
