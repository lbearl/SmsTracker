using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmsTracker.Data.Migrations
{
    public partial class MultiAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryPhone",
                table: "Accounts");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryPhone",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
