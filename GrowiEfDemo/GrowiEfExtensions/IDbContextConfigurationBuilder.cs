using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GrowiEfExtensions;

public interface IDbContextConfigurationBuilder<TContext> : IServiceCollection where TContext : DbContext
{
    /// <summary>
    /// Register all non-abstract enums, functions AND entity configurations matching predicate from the specified assembly to be applied to DbContext.
    /// </summary>
    /// <typeparam name="T">Marker type to specify what assembly to analyze.</typeparam>
    /// <param name="predicate">Optional filter: when specified, only types matching the predicate will be considered.</param>
    /// <returns>Instance of the builder.</returns>
    IDbContextConfigurationBuilder<TContext> RegisterAllConfigurationsFromAssembly<T>(Func<Type, bool>? predicate = null) =>
        RegisterAllConfigurationsFromAssembly(typeof(T).Assembly, predicate);

    /// <summary>
    /// Register all non-abstract enums, functions AND entity configurations matching predicate from the specified assembly to be applied to DbContext.
    /// </summary>
    /// <param name="assembly">Assembly to analyze.</param>
    /// <param name="predicate">Optional filter: when specified, only types matching the predicate will be considered.</param>
    /// <returns>Instance of the builder.</returns>
    IDbContextConfigurationBuilder<TContext> RegisterAllConfigurationsFromAssembly(Assembly assembly, Func<Type, bool>? predicate = null) =>
        RegisterAllEnumsFromAssembly(assembly, predicate)
            .RegisterAllRawSqlsFromAssembly(assembly, predicate)
            .RegisterAllFunctionsFromAssembly(assembly, predicate)
            .RegisterAllEntitiesFromAssembly(assembly, predicate);

    /// <summary>
    /// Register all non-abstract enums configurations matching predicate from the specified assembly to be applied to DbContext.
    /// </summary>
    /// <param name="assembly">Assembly to analyze.</param>
    /// <param name="predicate">Optional filter: when specified, only types matching the predicate will be considered.</param>
    /// <returns>Instance of the builder.</returns>
    IDbContextConfigurationBuilder<TContext> RegisterAllEnumsFromAssembly(Assembly assembly, Func<Type, bool>? predicate = null);

    /// <summary>
    /// Register all non-abstract raw SQL configurations matching predicate from the specified assembly to be applied to DbContext.
    /// </summary>
    /// <param name="assembly">Assembly to analyze.</param>
    /// <param name="predicate">Optional filter: when specified, only types matching the predicate will be considered.</param>
    /// <returns>Instance of the builder.</returns>
    IDbContextConfigurationBuilder<TContext> RegisterAllRawSqlsFromAssembly(Assembly assembly, Func<Type, bool>? predicate = null);

    /// <summary>
    /// Register all non-abstract functions configurations matching predicate from the specified assembly to be applied to DbContext.
    /// </summary>
    /// <param name="assembly">Assembly to analyze.</param>
    /// <param name="predicate">Optional filter: when specified, only types matching the predicate will be considered.</param>
    /// <returns>Instance of the builder.</returns>
    IDbContextConfigurationBuilder<TContext> RegisterAllFunctionsFromAssembly(Assembly assembly, Func<Type, bool>? predicate = null);

    /// <summary>
    /// Register all non-abstract entity configurations matching predicate from the specified assembly to be applied to DbContext.
    /// </summary>
    /// <param name="assembly">Assembly to analyze.</param>
    /// <param name="predicate">Optional filter: when specified, only types matching the predicate will be considered.</param>
    /// <returns>Instance of the builder.</returns>
    IDbContextConfigurationBuilder<TContext> RegisterAllEntitiesFromAssembly(Assembly assembly, Func<Type, bool>? predicate = null);

    /// <summary>
    /// Register concrete type of <see cref="IEnumsConfiguration"/> to be applied to DbContext.
    /// </summary>
    /// <param name="enumsConfiguration">Type of the configuration to use. MUST implement <see cref="IEnumsConfiguration"/>.</param>
    /// <returns>Instance of the builder.</returns>
    IDbContextConfigurationBuilder<TContext> RegisterEnums(Type enumsConfiguration);

    /// <summary>
    /// Register concrete type of <see cref="IRawSqlConfiguration"/> to be applied to DbContext.
    /// </summary>
    /// <param name="rawSqlConfiguration">Type of the configuration to use. MUST implement <see cref="IRawSqlConfiguration"/>.</param>
    /// <returns>Instance of the builder.</returns>
    IDbContextConfigurationBuilder<TContext> RegisterRawSql(Type rawSqlConfiguration);

    /// <summary>
    /// Register concrete type of <see cref="IFunctionsConfiguration"/> to be applied to DbContext.
    /// </summary>
    /// <param name="functionsConfiguration">Type of the configuration to use. MUST implement <see cref="IFunctionsConfiguration"/>.</param>
    /// <returns>Instance of the builder.</returns>
    IDbContextConfigurationBuilder<TContext> RegisterFunctions(Type functionsConfiguration);

    /// <summary>
    /// Register concrete type of <see cref="IEntityTypeConfiguration{TEntity}"/> to be applied to DbContext.
    /// </summary>
    /// <param name="entityConfiguration">Type of the configuration to use. MUST implement <see cref="IEntityTypeConfiguration{TEntity}"/>.</param>
    /// <returns>Instance of the builder.</returns>
    IDbContextConfigurationBuilder<TContext> RegisterEntity(Type entityConfiguration);

}
