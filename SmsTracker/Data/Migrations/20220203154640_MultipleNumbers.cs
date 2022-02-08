using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmsTracker.Data.Migrations
{
    public partial class MultipleNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Numbers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Numbers_AccountId",
                table: "Numbers",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Numbers_Accounts_AccountId",
                table: "Numbers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Numbers_Accounts_AccountId",
                table: "Numbers");

            migrationBuilder.DropIndex(
                name: "IX_Numbers_AccountId",
                table: "Numbers");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Numbers");
        }
    }
}
