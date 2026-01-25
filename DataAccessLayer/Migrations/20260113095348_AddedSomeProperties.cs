using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedSomeProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_Projects_ProjectId1",
                table: "ProjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_Users_UserId1",
                table: "ProjectUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskGroups_Users_CreatedById",
                table: "TaskGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_AssignedToId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_CreatedById",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_FinishedById",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_ProjectId1",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_UserId1",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ProjectUsers");

            migrationBuilder.AddColumn<string>(
                name: "ProfileColor",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ProjectUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskGroups_ProjectUsers_CreatedById",
                table: "TaskGroups",
                column: "CreatedById",
                principalTable: "ProjectUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ProjectUsers_AssignedToId",
                table: "Tasks",
                column: "AssignedToId",
                principalTable: "ProjectUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ProjectUsers_CreatedById",
                table: "Tasks",
                column: "CreatedById",
                principalTable: "ProjectUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ProjectUsers_FinishedById",
                table: "Tasks",
                column: "FinishedById",
                principalTable: "ProjectUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskGroups_ProjectUsers_CreatedById",
                table: "TaskGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ProjectUsers_AssignedToId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ProjectUsers_CreatedById",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ProjectUsers_FinishedById",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ProfileColor",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ProjectUsers");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId1",
                table: "ProjectUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "ProjectUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectId1",
                table: "ProjectUsers",
                column: "ProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId1",
                table: "ProjectUsers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_Projects_ProjectId1",
                table: "ProjectUsers",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_Users_UserId1",
                table: "ProjectUsers",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskGroups_Users_CreatedById",
                table: "TaskGroups",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_AssignedToId",
                table: "Tasks",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_CreatedById",
                table: "Tasks",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_FinishedById",
                table: "Tasks",
                column: "FinishedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
