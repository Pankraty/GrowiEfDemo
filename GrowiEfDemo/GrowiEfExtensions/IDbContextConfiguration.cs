using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

/// <summary>
/// Container for all configurations (both enums and entities) associated with the DbContext.
/// </summary>
public interface IDbContextConfiguration<TContext> where TContext : DbContext
{
    void ApplyAllConfigurations(ModelBuilder modelBuilder);
}
