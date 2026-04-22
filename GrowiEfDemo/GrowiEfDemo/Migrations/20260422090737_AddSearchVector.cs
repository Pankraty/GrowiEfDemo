using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace GrowiEfDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchVector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Companies_Inn",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Name",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Ogrn",
                table: "Companies");

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "Companies",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "to_tsvector('simple', \"Ogrn\") ||\r\nto_tsvector('simple', \"Inn\") ||\r\nto_tsvector('simple', \"Name\")",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_SearchVector",
                table: "Companies",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Companies_SearchVector",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "Companies");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Inn",
                table: "Companies",
                column: "Inn");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name",
                table: "Companies",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Ogrn",
                table: "Companies",
                column: "Ogrn");
        }
    }
}
