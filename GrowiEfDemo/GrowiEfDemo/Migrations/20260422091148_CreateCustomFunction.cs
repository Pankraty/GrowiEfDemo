using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowiEfDemo.Migrations
{
    /// <inheritdoc />
    public partial class CreateCustomFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE OR REPLACE FUNCTION websearch_to_prefixed_tsquery(query text)
                RETURNS tsquery
                LANGUAGE sql
                IMMUTABLE
                RETURN 
                    replace(
                        (websearch_to_tsquery('simple', query)::text || ' '),
                        ''' ', 
                        ''':* '
                    )::tsquery;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION websearch_to_prefixed_tsquery");
        }
    }
}
