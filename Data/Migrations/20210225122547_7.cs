using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_PenaltyCalculation.Data.Migrations
{
    public partial class _7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryWeekendNum1",
                table: "Country",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountryWeekendNum2",
                table: "Country",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryWeekendNum1",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CountryWeekendNum2",
                table: "Country");
        }
    }
}
