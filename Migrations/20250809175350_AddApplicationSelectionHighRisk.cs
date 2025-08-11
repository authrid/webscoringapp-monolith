using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScoringApp.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationSelectionHighRisk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HighRisk",
                table: "ApplicationSelections",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighRisk",
                table: "ApplicationSelections");
        }
    }
}
