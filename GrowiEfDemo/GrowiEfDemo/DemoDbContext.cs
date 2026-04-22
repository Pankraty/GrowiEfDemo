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
        modelBuilder.HasDbFunction(
            typeof(FullTextFunctions).GetMethod(nameof(FullTextFunctions.WebSearchToPrefixedTsQuery))!,
            b => b.HasName("websearch_to_prefixed_tsquery"));
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemoDbContext).Assembly);
    }
}