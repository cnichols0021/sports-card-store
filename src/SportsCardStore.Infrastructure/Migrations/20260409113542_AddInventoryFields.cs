using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsCardStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAutograph",
                table: "SportsCards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRelic",
                table: "SportsCards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRookie",
                table: "SportsCards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SetName",
                table: "SportsCards",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAutograph",
                table: "SportsCards");

            migrationBuilder.DropColumn(
                name: "IsRelic",
                table: "SportsCards");

            migrationBuilder.DropColumn(
                name: "IsRookie",
                table: "SportsCards");

            migrationBuilder.DropColumn(
                name: "SetName",
                table: "SportsCards");
        }
    }
}
