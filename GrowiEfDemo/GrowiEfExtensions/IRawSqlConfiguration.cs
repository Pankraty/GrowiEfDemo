using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

/// <summary>
/// Configuration for database objects managing via plain SQL scripts (such as functions, triggers, etc.).
/// </summary>
public interface IRawSqlConfiguration
{
    /// <summary>
    /// Raw SQL script to create or alter the object.
    /// </summary>
    string CreateScript();

    /// <summary>
    /// Raw SQL script to drop the object.
    /// </summary>
    /// <returns></returns>
    string DropScript();

    /// <summary>
    /// Appy configuration to the model builder.
    /// </summary>
    void ApplyConfiguration(ModelBuilder modelBuilder);
}
