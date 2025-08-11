using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScoringApp.Migrations
{
    /// <inheritdoc />
    public partial class AddItemOptionHighRisk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HighRisk",
                table: "ItemOptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighRisk",
                table: "ItemOptions");
        }
    }
}
