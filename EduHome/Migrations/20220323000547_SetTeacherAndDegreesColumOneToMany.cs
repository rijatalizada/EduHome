using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHome.Migrations
{
    public partial class SetTeacherAndDegreesColumOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DegreeId",
                table: "Teachers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DegreeId",
                table: "Teachers",
                column: "DegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Degrees_DegreeId",
                table: "Teachers",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Degrees_DegreeId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_DegreeId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "Teachers");
        }
    }
}
