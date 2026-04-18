using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;

namespace GrowiEfDemo;

public class DemoDbContext(DbContextOptions<DemoDbContext> options) : DbContext(options)
{
    public DbSet<Contract> Contracts { get; private set; } = null!;
    public DbSet<Company> Companies { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<RateMode>(nameTranslator: new NpgsqlNullNameTranslator());
        
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Key).IsUnique();
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Inn);
            entity.HasIndex(e => e.Ogrn);
        });
    }
}