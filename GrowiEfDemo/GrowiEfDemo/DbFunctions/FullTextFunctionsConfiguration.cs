using GrowiEfExtensions;

namespace GrowiEfDemo.DbFunctions;

public class SharedFunctionsConfiguration : FunctionsConfigurationBase
{
    public SharedFunctionsConfiguration()
    {
        RegisterDbFunction((string query) => FullTextFunctions.WebSearchToPrefixedTsQuery(query), "websearch_to_prefixed_tsquery");
    }
}