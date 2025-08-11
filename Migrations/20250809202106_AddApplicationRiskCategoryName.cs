using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScoringApp.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationRiskCategoryName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RiskCategoryName",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskCategoryName",
                table: "Applications");
        }
    }
}
