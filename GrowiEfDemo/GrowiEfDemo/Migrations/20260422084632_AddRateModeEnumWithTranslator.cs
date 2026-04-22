using GrowiEfDemo;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowiEfDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddRateModeEnumWithTranslator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:RateMode", "Yearly,Monthly,Daily");

            migrationBuilder.AddColumn<RateMode>(
                name: "RateMode",
                table: "Contracts",
                type: "\"RateMode\"",
                nullable: false,
                defaultValue: RateMode.Yearly);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RateMode",
                table: "Contracts");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:RateMode", "Yearly,Monthly,Daily");
        }
    }
}
