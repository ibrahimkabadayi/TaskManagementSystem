using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedTaskGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Sections_SectionId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Sections_SectionId1",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SectionId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "SectionId1",
                table: "Tasks",
                newName: "TaskGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_SectionId1",
                table: "Tasks",
                newName: "IX_Tasks_TaskGroupId");

            migrationBuilder.CreateTable(
                name: "TaskGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskGroups_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskGroups_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskGroups_CreatedById",
                table: "TaskGroups",
                column: "CreatedById",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskGroups_SectionId",
                table: "TaskGroups",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskGroups_TaskGroupId",
                table: "Tasks",
                column: "TaskGroupId",
                principalTable: "TaskGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskGroups_TaskGroupId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "TaskGroups");

            migrationBuilder.RenameColumn(
                name: "TaskGroupId",
                table: "Tasks",
                newName: "SectionId1");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TaskGroupId",
                table: "Tasks",
                newName: "IX_Tasks_SectionId1");

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SectionId",
                table: "Tasks",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Sections_SectionId",
                table: "Tasks",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Sections_SectionId1",
                table: "Tasks",
                column: "SectionId1",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
