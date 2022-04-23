using Microsoft.EntityFrameworkCore.Migrations;

namespace BugTracker.Data.Migrations
{
    public partial class projethandleuser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Handles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsrId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Handles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Handles_AspNetUsers_UsrId",
                        column: x => x.UsrId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Handles_Projects_ProjId",
                        column: x => x.ProjId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Handles_ProjId",
                table: "Handles",
                column: "ProjId");

            migrationBuilder.CreateIndex(
                name: "IX_Handles_UsrId",
                table: "Handles",
                column: "UsrId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Handles");
        }
    }
}
