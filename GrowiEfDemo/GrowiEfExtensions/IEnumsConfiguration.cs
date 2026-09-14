using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace GrowiEfExtensions;

/// <summary>
/// Allows configuration for enum types to be factored into a separate class,
/// rather than in-line in <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder)" />.
/// Implement this interface, applying configuration for the entity in the constructor, and then apply the configuration
/// to the model using <see cref="IEnumsConfiguration.ApplyConfiguration(M:Microsoft.EntityFrameworkCore.ModelBuilder)" />
/// in <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder)" />.
/// </summary>
public interface IEnumsConfiguration
{
    void ApplyConfiguration(ModelBuilder modelBuilder);
    public void RegisterEnum<T>(string? schemaName = null) where T : struct, Enum;

    NpgsqlDataSourceBuilder MapEnums(NpgsqlDataSourceBuilder dataSourceBuilder);
    NpgsqlDbContextOptionsBuilder MapEnums(NpgsqlDbContextOptionsBuilder dbContextOptionsBuilder);
}