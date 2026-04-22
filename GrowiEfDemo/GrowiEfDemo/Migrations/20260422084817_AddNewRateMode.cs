using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowiEfDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddNewRateMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:RateMode", "Yearly,Monthly,Weekly,Daily")
                .OldAnnotation("Npgsql:Enum:RateMode", "Yearly,Monthly,Daily");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:RateMode", "Yearly,Monthly,Daily")
                .OldAnnotation("Npgsql:Enum:RateMode", "Yearly,Monthly,Weekly,Daily");
        }
    }
}
