using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

public abstract class FunctionsConfigurationBase : IFunctionsConfiguration
{
    private readonly Dictionary<MethodInfo, string> _functions = new();

    protected void RegisterDbFunction(Expression expression, string dbName)
    {
        if (expression is not LambdaExpression lambdaExpression)
            throw new ArgumentException("Expression is not a lambda expression");
        if (lambdaExpression.Body is not MethodCallExpression methodCallExpression)
            throw new ArgumentException("Expression must be a lambda expression with a single method call");


        _functions[methodCallExpression.Method] = dbName;
    }

    public void ApplyConfiguration(ModelBuilder modelBuilder)
    {
        foreach (var pair in _functions)
        {
            var (Schema, FunctionName) = Split(pair.Value);
            var functionBuilder = modelBuilder.HasDbFunction(pair.Key).HasName(FunctionName);
            if (Schema is not null)
                functionBuilder.HasSchema(Schema);
        }

        return;

        (string? Schema, string FunctionName) Split(string fullName)
        {
            var index = fullName.IndexOf('.', StringComparison.Ordinal);
            return index > 0
                ? (fullName[..index], fullName[(index + 1)..])
                : (null, fullName);
        }
    }
}
