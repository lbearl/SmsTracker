using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmsTracker.Data.Migrations
{
    public partial class SmsPrefixSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "Accounts",
                type: "TEXT",
                maxLength: 5,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "Accounts");
        }
    }
}
