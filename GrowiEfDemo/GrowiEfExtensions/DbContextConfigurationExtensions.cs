using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GrowiEfExtensions;

public static class DbContextConfigurationExtensions
{
    /// <summary>
    /// Starting method for configuring DbContext by applying specified configuration for entity types and enums.
    /// </summary>
    /// <param name="services">Services collection.</param>
    /// <typeparam name="TContext">Type of <see cref="DbContext"/> to apply following configurations to.</typeparam>
    /// <returns>Instance of builder to chain configuration method calls.</returns>
    public static IDbContextConfigurationBuilder<TContext> Configure<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddScoped<IDbContextConfiguration<TContext>, DbContextConfiguration<TContext>>();
        return new DbContextConfigurationBuilder<TContext>(services);
    }
}
