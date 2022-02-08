using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmsTracker.Data.Migrations
{
    public partial class MultipleNumbers1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Numbers_Accounts_AccountId",
                table: "Numbers");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Numbers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Numbers_Accounts_AccountId",
                table: "Numbers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Numbers_Accounts_AccountId",
                table: "Numbers");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Numbers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Numbers_Accounts_AccountId",
                table: "Numbers",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
