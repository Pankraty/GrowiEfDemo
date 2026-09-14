using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowiEfDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddRateMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RateMode",
                table: "Contracts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RateMode",
                table: "Contracts");
        }
    }
}
