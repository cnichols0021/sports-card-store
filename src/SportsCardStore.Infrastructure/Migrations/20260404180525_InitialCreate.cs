using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsCardStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SportsCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Sport = table.Column<int>(type: "int", nullable: false),
                    Team = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Grade = table.Column<decimal>(type: "decimal(3,1)", nullable: true),
                    GradingCompany = table.Column<int>(type: "int", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportsCards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SportsCards_CreatedDate",
                table: "SportsCards",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_SportsCards_IsAvailable",
                table: "SportsCards",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_SportsCards_PlayerName",
                table: "SportsCards",
                column: "PlayerName");

            migrationBuilder.CreateIndex(
                name: "IX_SportsCards_PlayerName_Year",
                table: "SportsCards",
                columns: new[] { "PlayerName", "Year" });

            migrationBuilder.CreateIndex(
                name: "IX_SportsCards_Sport",
                table: "SportsCards",
                column: "Sport");

            migrationBuilder.CreateIndex(
                name: "IX_SportsCards_Year",
                table: "SportsCards",
                column: "Year");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SportsCards");
        }
    }
}
