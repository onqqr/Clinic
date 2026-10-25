using Clinic.Services.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Services.Infrastructure.Data;

public class ServicesDbContext : DbContext
{
    public ServicesDbContext(DbContextOptions<ServicesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Specialization> Specializations => Set<Specialization>();

    public DbSet<ServiceItem> ServiceItems => Set<ServiceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.ToTable("Specializations");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Status).HasConversion<string>();

            entity.HasMany(x => x.Services)
                .WithOne(x => x.Specialization!)
                .HasForeignKey(x => x.SpecializationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ServiceItem>(entity =>
        {
            entity.ToTable("Services");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Price).HasPrecision(18, 2);
            entity.Property(x => x.Category).HasConversion<string>();
            entity.Property(x => x.Status).HasConversion<string>();
        });
    }
}
