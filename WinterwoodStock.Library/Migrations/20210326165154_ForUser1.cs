using Microsoft.EntityFrameworkCore.Migrations;

namespace WinterwoodStock.Library.Migrations
{
    public partial class ForUser1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "CreatedUser",
                table: "Users",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "CreatedUser");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "Batches",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
