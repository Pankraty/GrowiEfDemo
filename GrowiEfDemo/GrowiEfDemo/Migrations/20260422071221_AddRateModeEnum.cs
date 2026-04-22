using GrowiEfDemo;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowiEfDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddRateModeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:rate_mode", "daily,monthly,yearly");

            migrationBuilder.AddColumn<RateMode>(
                name: "RateMode",
                table: "Contracts",
                type: "rate_mode",
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
                .OldAnnotation("Npgsql:Enum:rate_mode", "daily,monthly,yearly");
        }
    }
}
