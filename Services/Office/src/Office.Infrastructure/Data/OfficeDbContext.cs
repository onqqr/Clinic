using Microsoft.EntityFrameworkCore;
using Clinic.Offices.Domain.Entities;
namespace Clinic.Offices.Infrastructure.Data;

public class OfficeDbContext : DbContext
{
    public OfficeDbContext(DbContextOptions<OfficeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Office> Offices => Set<Office>();
}