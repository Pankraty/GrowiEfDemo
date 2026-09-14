using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

public abstract class RawSqlConfigurationBase : IRawSqlConfiguration
{
    private const string _prefix = "Growi:Functions:";
    public const string PrefixCreate = $"{_prefix}Create:";
    public const string PrefixDrop = $"{_prefix}Drop:";

    public abstract string CreateScript();
    public abstract string DropScript();

    public void ApplyConfiguration(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        var keyCreate = $"{PrefixCreate}{GetType().FullName}";
        var keyDrop = $"{PrefixDrop}{GetType().FullName}";
        modelBuilder.HasAnnotation(keyCreate, CreateScript());
        modelBuilder.HasAnnotation(keyDrop, DropScript());
    }
}
