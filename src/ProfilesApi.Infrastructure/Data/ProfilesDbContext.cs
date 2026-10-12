namespace ProfilesApi.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using ProfilesApi.Domain.Entities;

public class ProfilesDbContext : DbContext
{
    public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Doctor> Doctors => Set<Doctor>();

    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<Receptionist> Receptionists => Set<Receptionist>();
}