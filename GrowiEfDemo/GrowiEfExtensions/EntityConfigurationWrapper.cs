using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

/// <summary>
/// Container establishing connection between <see cref="DbContext"/> and concrete <paramref name="configuration"/>.
/// </summary>
/// <typeparam name="TContext">Type of the DbContext.</typeparam>
internal sealed class EntityConfigurationWrapper<TContext, TConfiguration, TEntity>(TConfiguration configuration)
    : IEntityConfigurationWrapper<TContext>
    where TContext : DbContext
    where TConfiguration : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    public void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(configuration);
    }
}
