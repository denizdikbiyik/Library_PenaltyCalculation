using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_PenaltyCalculation.Data.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryMoneyType",
                table: "Country",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryMoneyType",
                table: "Country");
        }
    }
}
