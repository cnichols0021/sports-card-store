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
                    PlayerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SetName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CardNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Sport = table.Column<int>(type: "int", nullable: false),
                    Team = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsRookie = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsAutograph = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsRelic = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsBowmanFirst = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    ParallelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PrintRun = table.Column<int>(type: "int", nullable: true),
                    Grade = table.Column<float>(type: "REAL", nullable: true),
                    GradingCompany = table.Column<int>(type: "int", nullable: false),
                    Condition = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Price = table.Column<float>(type: "REAL", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<string>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedDate = table.Column<string>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
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
