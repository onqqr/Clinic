using Clinic.Offices.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOfficeServices(builder.Configuration);

var app = builder.Build();

app.MapControllers();

app.Run();
