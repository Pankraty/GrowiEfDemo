using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

/// <summary>
/// Container establishing connection between <see cref="DbContext"/> and <see cref="IEntityTypeConfiguration{TEntity}"/>.
/// </summary>
/// <typeparam name="TContext">Type of the DbContext.</typeparam>
internal interface IEntityConfigurationWrapper<TContext> where TContext : DbContext
{
    void Apply(ModelBuilder modelBuilder);
}
