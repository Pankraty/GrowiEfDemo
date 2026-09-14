using System.Collections;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GrowiEfExtensions;

/// <summary>
/// Implementation of <see cref="IDbContextConfigurationBuilder{TContext}"/>. Implements <see cref="IServiceCollection"/>
/// to ease methods chaining.
/// </summary>
internal sealed class DbContextConfigurationBuilder<TContext>(IServiceCollection services)
    : IDbContextConfigurationBuilder<TContext>
    where TContext : DbContext
{
    private static readonly Type _openGenericEnumType = typeof(EnumConfigurationWrapper<,>);
    private static readonly Type _openGenericFunctionType = typeof(FunctionConfigurationWrapper<,>);
    private static readonly Type _openGenericRawSqlType = typeof(RawSqlConfigurationWrapper<,>);
    private static readonly Type _openGenericEntityType = typeof(EntityConfigurationWrapper<,,>);

    /// <inheritdoc />
    public IDbContextConfigurationBuilder<TContext> RegisterAllEnumsFromAssembly(Assembly assembly,
        Func<Type, bool>? predicate = null)
    {
        var configurationTypes = assembly.DefinedTypes
            .Where(typeInfo => typeInfo is { IsAbstract: false, IsGenericTypeDefinition: false }
                               && typeInfo.GetInterfaces().Any(i => i == typeof(IEnumsConfiguration))
                               && (predicate == null || predicate.Invoke(typeInfo)));

        foreach (var configurationType in configurationTypes)
        {
            RegisterEnums(configurationType);
        }

        return this;
    }

    /// <inheritdoc />
    public IDbContextConfigurationBuilder<TContext> RegisterAllFunctionsFromAssembly(Assembly assembly,
        Func<Type, bool>? predicate = null)
    {
        var configurationTypes = assembly.DefinedTypes
            .Where(typeInfo => typeInfo is { IsAbstract: false, IsGenericTypeDefinition: false }
                               && typeInfo.GetInterfaces().Any(i => i == typeof(IFunctionsConfiguration))
                               && (predicate == null || predicate.Invoke(typeInfo)));

        foreach (var configurationType in configurationTypes)
        {
            RegisterFunctions(configurationType);
        }

        return this;
    }

    /// <inheritdoc />
    public IDbContextConfigurationBuilder<TContext> RegisterAllRawSqlsFromAssembly(Assembly assembly,
        Func<Type, bool>? predicate = null)
    {
        var configurationTypes = assembly.DefinedTypes
            .Where(typeInfo => typeInfo is { IsAbstract: false, IsGenericTypeDefinition: false }
                               && typeInfo.GetInterfaces().Any(i => i == typeof(IRawSqlConfiguration))
                               && (predicate == null || predicate.Invoke(typeInfo)));

        foreach (var configurationType in configurationTypes)
        {
            RegisterRawSql(configurationType);
        }

        return this;
    }

    /// <inheritdoc />
    public IDbContextConfigurationBuilder<TContext> RegisterAllEntitiesFromAssembly(Assembly assembly, Func<Type, bool>? predicate = null)
    {
        var configurationTypes = assembly.DefinedTypes
            .Where(typeInfo => typeInfo is { IsAbstract: false, IsGenericTypeDefinition: false }
                               && typeInfo.GetInterfaces().Any(i => i.IsGenericType &&
                                                                    i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                               && (predicate == null || predicate.Invoke(typeInfo)));

        foreach (var configurationType in configurationTypes)
        {
            RegisterEntity(configurationType);
        }

        return this;
    }

    /// <inheritdoc />
    public IDbContextConfigurationBuilder<TContext> RegisterEnums(Type enumsConfiguration)
    {
        var wrapperType = _openGenericEnumType.MakeGenericType([typeof(TContext), enumsConfiguration]);

        services.AddScoped(typeof(IEnumConfigurationWrapper<TContext>), wrapperType);
        services.AddScoped(enumsConfiguration);
        return this;
    }

    /// <inheritdoc />
    public IDbContextConfigurationBuilder<TContext> RegisterFunctions(Type functionsConfiguration)
    {
        var wrapperType = _openGenericFunctionType.MakeGenericType([typeof(TContext), functionsConfiguration]);

        services.AddScoped(typeof(IFunctionsConfigurationWrapper<TContext>), wrapperType);
        services.AddScoped(functionsConfiguration);
        return this;
    }

    /// <inheritdoc />
    public IDbContextConfigurationBuilder<TContext> RegisterRawSql(Type rawSqlConfiguration)
    {
        var wrapperType = _openGenericRawSqlType.MakeGenericType([typeof(TContext), rawSqlConfiguration]);

        services.AddScoped(typeof(IRawSqlConfigurationWrapper<TContext>), wrapperType);
        services.AddScoped(rawSqlConfiguration);
        return this;
    }

    /// <inheritdoc />
    public IDbContextConfigurationBuilder<TContext> RegisterEntity(Type entityConfiguration)
    {
        var entityType = (entityConfiguration.GetInterfaces()
                              .SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                          ?? throw new ArgumentException($"Entity type {entityConfiguration.Name} does not implement IEntityTypeConfiguration."))
            .GetGenericArguments()[0];
        var wrapperType = _openGenericEntityType.MakeGenericType([typeof(TContext), entityConfiguration, entityType]);

        services.AddScoped(typeof(IEntityConfigurationWrapper<TContext>), wrapperType);
        services.AddScoped(entityConfiguration);
        return this;
    }

    #region IServiceCollection implementation
    public IEnumerator<ServiceDescriptor> GetEnumerator()
    {
        return services.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)services).GetEnumerator();
    }

    public void Add(ServiceDescriptor item)
    {
        services.Add(item);
    }

    public void Clear()
    {
        services.Clear();
    }

    public bool Contains(ServiceDescriptor item)
    {
        return services.Contains(item);
    }

    public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
    {
        services.CopyTo(array, arrayIndex);
    }

    public bool Remove(ServiceDescriptor item)
    {
        return services.Remove(item);
    }

    public int Count => services.Count;

    public bool IsReadOnly => services.IsReadOnly;

    public int IndexOf(ServiceDescriptor item)
    {
        return services.IndexOf(item);
    }

    public void Insert(int index, ServiceDescriptor item)
    {
        services.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        services.RemoveAt(index);
    }

    public ServiceDescriptor this[int index]
    {
        get => services[index];
        set => services[index] = value;
    }

    #endregion IServiceCollection implementation
}
