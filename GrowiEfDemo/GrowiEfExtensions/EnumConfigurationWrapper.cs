using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

/// <summary>
/// Container establishing connection between <see cref="DbContext"/> and concrete <paramref name="configuration"/>.
/// </summary>
/// <typeparam name="TContext">Type of the DbContext.</typeparam>
internal sealed class EnumConfigurationWrapper<TContext, TConfiguration>(TConfiguration configuration)
    : IEnumConfigurationWrapper<TContext>
    where TContext : DbContext
    where TConfiguration : IEnumsConfiguration
{
    public void Apply(ModelBuilder modelBuilder)
    {
        configuration.ApplyConfiguration(modelBuilder);
    }
}
