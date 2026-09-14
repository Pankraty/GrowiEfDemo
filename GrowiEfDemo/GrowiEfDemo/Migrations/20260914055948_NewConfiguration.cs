using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowiEfDemo.Migrations
{
    /// <inheritdoc />
    public partial class NewConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("create or replace function websearch_to_prefixed_tsquery(query text)\r\nreturns tsquery\r\nimmutable\r\nreturn replace(\r\n    websearch_to_tsquery('simple', query)::text || ' ',\r\n    ''' ',\r\n    ''':*'\r\n  )::tsquery;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop function websearch_to_prefixed_tsquery(text);");
        }
    }
}
