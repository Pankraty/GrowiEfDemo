using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace GrowiEfExtensions;

#pragma warning disable EF1001
public class CustomDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MigrationsModelDiffer>();
        serviceCollection.AddSingleton<IMigrationsModelDiffer>(sp => new RawSqlMigrationsModelDiffer(sp.GetRequiredService<MigrationsModelDiffer>()));
    }
}

internal sealed class RawSqlMigrationsModelDiffer(MigrationsModelDiffer innerModelDiffer) : IMigrationsModelDiffer
{
    public bool HasDifferences(IRelationalModel? source, IRelationalModel? target)
    {
        return innerModelDiffer.HasDifferences(source, target);
    }

    public IReadOnlyList<MigrationOperation> GetDifferences(IRelationalModel? source, IRelationalModel? target)
    {
        var operations = innerModelDiffer.GetDifferences(source, target);
        var functionOperations = GetRawSqlDifferences(source, target);

        return operations.Concat(functionOperations).ToList();
    }

    private IEnumerable<MigrationOperation> GetRawSqlDifferences(IRelationalModel? source, IRelationalModel? target)
    {
        var sourceDefinitions = GetRawSqlDefinitionsFromAnnotations(source);
        var targetDefinitions = GetRawSqlDefinitionsFromAnnotations(target);

        var definitionsToAlter = targetDefinitions
            .Where(x => x.Value != sourceDefinitions.GetValueOrDefault(x.Key))
            .OrderBy(x => x.Key)
            .Select(x => x.Value)
            .ToList();

        foreach (var sql in definitionsToAlter.Where(sql => !string.IsNullOrEmpty(sql)))
        {
            yield return new SqlOperation
            {
                Sql = sql
            };
        }

        var definitionsToDrop = sourceDefinitions.Keys.Except(targetDefinitions.Keys)
            .OrderDescending()
            .ToList();
        foreach (var functionKey in definitionsToDrop)
        {
            var sql = source!.Model.GetAnnotation(RawSqlConfigurationBase.PrefixDrop + functionKey).Value?.ToString();

            if (!string.IsNullOrEmpty(sql))
            {
                yield return new SqlOperation
                {
                    Sql = sql
                };
            }
        }

        Dictionary<string, string> GetRawSqlDefinitionsFromAnnotations(IRelationalModel? model) => model?.Model
            .GetAnnotations()
            .Where(x => x.Name.StartsWith(RawSqlConfigurationBase.PrefixCreate, StringComparison.Ordinal))
            .ToDictionary(
                x => x.Name[RawSqlConfigurationBase.PrefixCreate.Length..],
                x => x.Value?.ToString() ?? "") ?? [];
    }
}
#pragma warning restore EF1001
