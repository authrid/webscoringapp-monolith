using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebScoringApp.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationSelection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSelections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    GroupItemId = table.Column<int>(type: "integer", nullable: false),
                    ItemOptionId = table.Column<int>(type: "integer", nullable: false),
                    Bobot = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSelections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationSelections_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationSelections_GroupItems_GroupItemId",
                        column: x => x.GroupItemId,
                        principalTable: "GroupItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationSelections_ItemOptions_ItemOptionId",
                        column: x => x.ItemOptionId,
                        principalTable: "ItemOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSelections_ApplicationId",
                table: "ApplicationSelections",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSelections_GroupItemId",
                table: "ApplicationSelections",
                column: "GroupItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSelections_ItemOptionId",
                table: "ApplicationSelections",
                column: "ItemOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSelections");
        }
    }
}
