using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsCardStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBowmanFirstField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBowmanFirst",
                table: "SportsCards",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBowmanFirst",
                table: "SportsCards");
        }
    }
}
