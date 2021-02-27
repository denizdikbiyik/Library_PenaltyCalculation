using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_PenaltyCalculation.Data.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalculatedBusinessDays",
                table: "ReturnBook",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "CalculatedPenalty",
                table: "ReturnBook",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalculatedBusinessDays",
                table: "ReturnBook");

            migrationBuilder.DropColumn(
                name: "CalculatedPenalty",
                table: "ReturnBook");
        }
    }
}
