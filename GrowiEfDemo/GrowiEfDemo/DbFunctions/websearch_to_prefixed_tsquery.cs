using GrowiEfExtensions;

namespace GrowiEfDemo.DbFunctions;

public class websearch_to_prefixed_tsquery : RawSqlConfigurationBase
{
    public override string CreateScript() =>
        """
        create or replace function websearch_to_prefixed_tsquery(query text)
        returns tsquery
        immutable
        return replace(
            websearch_to_tsquery('simple', query)::text || ' ',
            ''' ',
            ''':*'
          )::tsquery;
        """;

    public override string DropScript() =>
        """
        drop function websearch_to_prefixed_tsquery(text);
        """;
}