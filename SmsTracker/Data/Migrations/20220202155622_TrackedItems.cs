using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmsTracker.Data.Migrations
{
    public partial class TrackedItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrackedItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    OwnedByAccountId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedItems_Accounts_OwnedByAccountId",
                        column: x => x.OwnedByAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackedItems_OwnedByAccountId",
                table: "TrackedItems",
                column: "OwnedByAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackedItems");
        }
    }
}
