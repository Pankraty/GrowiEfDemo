using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace GrowiEfExtensions;

public static class ConfigurationsProcessor
{
    /// <summary>
    /// Applies configuration from all <see cref="IEnumsConfiguration" /> instances that are defined in provided assembly.
    /// </summary>
    public static void MapAllConfiguredEnums(this NpgsqlDataSourceBuilder dataSourceBuilder, Assembly assembly, Func<Type, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        foreach (var instance in GetAllEnumConfigurations(assembly, predicate))
        {
            instance.MapEnums(dataSourceBuilder);
        }
    }

    /// <summary>
    /// Applies configuration from all <see cref="IEnumsConfiguration" /> instances that are defined in provided assembly.
    /// </summary>
    public static NpgsqlDbContextOptionsBuilder MapAllConfiguredEnums(this NpgsqlDbContextOptionsBuilder dbContextOptionsBuilder, Assembly assembly, Func<Type, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        foreach (var instance in GetAllEnumConfigurations(assembly, predicate))
        {
            instance.MapEnums(dbContextOptionsBuilder);
        }
        return dbContextOptionsBuilder;
    }

    /// <summary>
    /// Applies configuration from all <see cref="IEnumsConfiguration" /> instances that are defined in provided assembly.
    /// </summary>
    public static ModelBuilder ApplyEnumConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, Func<Type, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        foreach (var instance in GetAllEnumConfigurations(assembly, predicate))
        {
            instance.ApplyConfiguration(modelBuilder);
        }

        return modelBuilder;
    }

    /// <summary>
    /// Applies configuration from all <see cref="IRawSqlConfiguration" /> instances that are defined in provided assembly.
    /// </summary>
    public static ModelBuilder ApplyRawSqlConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, Func<Type, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        foreach (var instance in GetAllRawSqlConfigurations(assembly, predicate))
        {
            instance.ApplyConfiguration(modelBuilder);
        }

        return modelBuilder;
    }

    /// <summary>
    /// Applies configuration from all <see cref="IFunctionsConfiguration" /> instances that are defined in provided assembly.
    /// </summary>
    public static ModelBuilder ApplyFunctionConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, Func<Type, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        foreach (var instance in GetAllFunctionConfigurations(assembly, predicate))
        {
            instance.ApplyConfiguration(modelBuilder);
        }

        return modelBuilder;
    }

    /// <summary>
    /// Applies configuration from all <see cref="IEntityTypeConfiguration{TEntity}" /> AND <see cref="IEnumsConfiguration" />
    /// instances that are defined in provided assembly.
    /// </summary>
    public static ModelBuilder ApplyAllConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly, Func<Type, bool>? predicate = null)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        ArgumentNullException.ThrowIfNull(assembly);
        return modelBuilder
            .ApplyEnumConfigurationsFromAssembly(assembly)
            .ApplyRawSqlConfigurationsFromAssembly(assembly)
            .ApplyFunctionConfigurationsFromAssembly(assembly)
            .ApplyConfigurationsFromAssembly(assembly, predicate);
    }

    private static IEnumerable<IEnumsConfiguration> GetAllEnumConfigurations(Assembly assembly,
        Func<Type, bool>? predicate = null) => assembly.DefinedTypes
        .Where(typeInfo => typeInfo is { IsAbstract: false, IsGenericTypeDefinition: false } &&
                           typeInfo.GetConstructor(Type.EmptyTypes) != null
                           || (!predicate?.Invoke(typeInfo) ?? false)
                           || typeInfo.GetInterfaces().Any(i => i == typeof(IEnumsConfiguration)))
        .Select(type => Activator.CreateInstance(type) as IEnumsConfiguration)
        .OfType<IEnumsConfiguration>();

    private static IEnumerable<IRawSqlConfiguration> GetAllRawSqlConfigurations(Assembly assembly,
        Func<Type, bool>? predicate = null) => assembly.DefinedTypes
        .Where(typeInfo => typeInfo is { IsAbstract: false, IsGenericTypeDefinition: false } &&
                           typeInfo.GetConstructor(Type.EmptyTypes) != null
                           || (!predicate?.Invoke(typeInfo) ?? false)
                           || typeInfo.GetInterfaces().Any(i => i == typeof(IRawSqlConfiguration)))
        .Select(type => Activator.CreateInstance(type) as IRawSqlConfiguration)
        .OfType<IRawSqlConfiguration>();

    private static IEnumerable<IFunctionsConfiguration> GetAllFunctionConfigurations(Assembly assembly,
        Func<Type, bool>? predicate = null) => assembly.DefinedTypes
        .Where(typeInfo => typeInfo is { IsAbstract: false, IsGenericTypeDefinition: false } &&
                           typeInfo.GetConstructor(Type.EmptyTypes) != null
                           || (!predicate?.Invoke(typeInfo) ?? false)
                           || typeInfo.GetInterfaces().Any(i => i == typeof(IFunctionsConfiguration)))
        .Select(type => Activator.CreateInstance(type) as IFunctionsConfiguration)
        .OfType<IFunctionsConfiguration>();
}