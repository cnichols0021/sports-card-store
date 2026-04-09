using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsCardStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddParallelFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParallelName",
                table: "SportsCards",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrintRun",
                table: "SportsCards",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParallelName",
                table: "SportsCards");

            migrationBuilder.DropColumn(
                name: "PrintRun",
                table: "SportsCards");
        }
    }
}
