using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FiorelloBackend.Migrations
{
    public partial class UpdateCreateHomeAboutsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "HomeAbouts");

            migrationBuilder.DropColumn(
                name: "IconContent",
                table: "HomeAbouts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "HomeAbouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IconContent",
                table: "HomeAbouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
