using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Npgsql.NameTranslation;

namespace GrowiEfExtensions;

/// <summary>
/// Standard implementation of <see cref="IEnumsConfiguration"/> that uses <see cref="NpgsqlNullNameTranslator"/>.
/// </summary>
public abstract class EnumsConfigurationBase : IEnumsConfiguration
{
    private static readonly INpgsqlNameTranslator _nullNameTranslator = new NpgsqlNullNameTranslator();

    private readonly List<Action<NpgsqlDataSourceBuilder>> _mapEnumToDataSourceDelegates = new();
    private readonly List<Action<NpgsqlDbContextOptionsBuilder>> _mapEnumToOptionsDelegates = new();
    private readonly List<Action<ModelBuilder>> _configureDelegates = new();
    public void RegisterEnum<T>(string? schemaName = null) where T : struct, Enum
    {
        var typeName = typeof(T).Name;
        var pgName = string.IsNullOrEmpty(schemaName)
            ? typeName
            : $"{schemaName}.{typeName}";

        _mapEnumToDataSourceDelegates.Add(dataSourceBuilder => dataSourceBuilder.MapEnum<T>(pgName: pgName, _nullNameTranslator));
        _mapEnumToOptionsDelegates.Add(optionsBuilder => optionsBuilder.MapEnum<T>(schemaName: schemaName, nameTranslator: _nullNameTranslator));
        _configureDelegates.Add(modelBuilder => modelBuilder.HasPostgresEnum<T>(schema: schemaName, nameTranslator: _nullNameTranslator));
    }

    public NpgsqlDataSourceBuilder MapEnums(NpgsqlDataSourceBuilder dataSourceBuilder)
    {
        foreach (var mapEnum in _mapEnumToDataSourceDelegates)
        {
            mapEnum.Invoke(dataSourceBuilder);
        }

        return dataSourceBuilder;
    }

    public NpgsqlDbContextOptionsBuilder MapEnums(NpgsqlDbContextOptionsBuilder dbContextOptionsBuilder)
    {
        foreach (var mapEnum in _mapEnumToOptionsDelegates)
        {
            mapEnum.Invoke(dbContextOptionsBuilder);
        }

        return dbContextOptionsBuilder;
    }

    public void ApplyConfiguration(ModelBuilder modelBuilder)
    {
        foreach (var configure in _configureDelegates)
        {
            configure.Invoke(modelBuilder);
        }
    }
}