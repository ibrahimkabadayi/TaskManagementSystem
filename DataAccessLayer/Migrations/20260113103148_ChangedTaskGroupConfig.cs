using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTaskGroupConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskGroups_CreatedById",
                table: "TaskGroups");

            migrationBuilder.CreateIndex(
                name: "IX_TaskGroups_CreatedById",
                table: "TaskGroups",
                column: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TaskGroups_CreatedById",
                table: "TaskGroups");

            migrationBuilder.CreateIndex(
                name: "IX_TaskGroups_CreatedById",
                table: "TaskGroups",
                column: "CreatedById",
                unique: true);
        }
    }
}
