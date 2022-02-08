using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmsTracker.Data.Migrations
{
    public partial class UserAccountLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnedByUserId",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_OwnedByUserId",
                table: "Accounts",
                column: "OwnedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AspNetUsers_OwnedByUserId",
                table: "Accounts",
                column: "OwnedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AspNetUsers_OwnedByUserId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_OwnedByUserId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "OwnedByUserId",
                table: "Accounts");
        }
    }
}
