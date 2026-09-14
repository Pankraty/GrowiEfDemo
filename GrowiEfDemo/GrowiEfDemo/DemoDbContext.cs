using GrowiEfExtensions;
using Microsoft.EntityFrameworkCore;

namespace GrowiEfDemo;

public class DemoDbContext(DbContextOptions<DemoDbContext> options,
    IDbContextConfiguration<DemoDbContext> dbContextConfiguration) : DbContext(options)
{
    public DbSet<Contract> Contracts { get; private set; } = null!;
    public DbSet<Company> Companies { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        dbContextConfiguration.ApplyAllConfigurations(modelBuilder);
    }
}