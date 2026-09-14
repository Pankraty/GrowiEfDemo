using Microsoft.EntityFrameworkCore.Diagnostics;
using NpgsqlTypes;

namespace GrowiEfDemo;

public static class FullTextFunctions
{
    public static NpgsqlTsQuery WebSearchToPrefixedTsQuery(string query)
        => throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(WebSearchToPrefixedTsQuery)));
}