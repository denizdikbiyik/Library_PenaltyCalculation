using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_PenaltyCalculation.Data.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CalculatedPenalty",
                table: "ReturnBook",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "CalculatedPenalty",
                table: "ReturnBook",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
