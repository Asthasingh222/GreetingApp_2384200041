using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    public partial class AddUserForeignKeyToGreeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Greetings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Greetings_UserId",
                table: "Greetings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Greetings_Users_UserId",
                table: "Greetings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Greetings_Users_UserId",
                table: "Greetings");

            migrationBuilder.DropIndex(
                name: "IX_Greetings_UserId",
                table: "Greetings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Greetings");
        }
    }
}
