using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

/// <summary>
/// Container establishing connection between <see cref="DbContext"/> and concrete <paramref name="configuration"/>.
/// </summary>
/// <typeparam name="TContext">Type of the DbContext.</typeparam>
internal sealed class FunctionConfigurationWrapper<TContext, TConfiguration>(TConfiguration configuration)
    : IFunctionsConfigurationWrapper<TContext>
    where TContext : DbContext
    where TConfiguration : IFunctionsConfiguration
{
    public void Apply(ModelBuilder modelBuilder)
    {
        configuration.ApplyConfiguration(modelBuilder);
    }
}
