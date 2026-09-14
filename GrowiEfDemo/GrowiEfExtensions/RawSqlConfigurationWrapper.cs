using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

/// <summary>
/// Container establishing connection between <see cref="DbContext"/> and concrete <paramref name="configuration"/>.
/// </summary>
/// <typeparam name="TContext">Type of the DbContext.</typeparam>
internal sealed class RawSqlConfigurationWrapper<TContext, TConfiguration>(TConfiguration configuration)
    : IRawSqlConfigurationWrapper<TContext>
    where TContext : DbContext
    where TConfiguration : IRawSqlConfiguration
{
    public void Apply(ModelBuilder modelBuilder)
    {
        configuration.ApplyConfiguration(modelBuilder);
    }
}
