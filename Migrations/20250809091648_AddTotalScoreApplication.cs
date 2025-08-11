using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScoringApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalScoreApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalScore",
                table: "Applications",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "Applications");
        }
    }
}
