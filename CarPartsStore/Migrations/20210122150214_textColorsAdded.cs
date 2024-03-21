using Microsoft.EntityFrameworkCore.Migrations;

namespace CarPartsStore.Migrations
{
    public partial class textColorsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstTextColor",
                table: "HeadSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondTextColor",
                table: "HeadSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstTextColor",
                table: "HeadSettings");

            migrationBuilder.DropColumn(
                name: "SecondTextColor",
                table: "HeadSettings");
        }
    }
}
