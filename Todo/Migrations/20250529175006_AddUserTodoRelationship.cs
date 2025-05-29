using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTodoRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Todos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Todos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_AppUserId",
                table: "Todos",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_AspNetUsers_AppUserId",
                table: "Todos",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_AspNetUsers_AppUserId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_AppUserId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Todos");
        }
    }
}
