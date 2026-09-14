using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

/// <summary>
/// Container for all configurations (enums, functions and entities) associated with the DbContext.
/// </summary>
internal sealed class DbContextConfiguration<TContext>(
    IEnumerable<IEnumConfigurationWrapper<TContext>> enumConfigurations,
    IEnumerable<IRawSqlConfigurationWrapper<TContext>> rawSqlConfigurations,
    IEnumerable<IFunctionsConfigurationWrapper<TContext>> functionConfigurations,
    IEnumerable<IEntityConfigurationWrapper<TContext>> entityConfigurations)
    : IDbContextConfiguration<TContext> where TContext : DbContext
{
    /// <summary>
    /// Apply all configurations to the <paramref name="modelBuilder"/>.
    /// </summary>
    public void ApplyAllConfigurations(ModelBuilder modelBuilder)
    {
        foreach (var enumConfiguration in enumConfigurations)
        {
            enumConfiguration.Apply(modelBuilder);
        }

        foreach (var rawSqlConfiguration in rawSqlConfigurations)
        {
            rawSqlConfiguration.Apply(modelBuilder);
        }

        foreach (var functionConfiguration in functionConfigurations)
        {
            functionConfiguration.Apply(modelBuilder);
        }

        foreach (var entityConfiguration in entityConfigurations)
        {
            entityConfiguration.Apply(modelBuilder);
        }
    }
}
