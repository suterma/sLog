using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sLog.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    RegistrationToken = table.Column<Guid>(nullable: false),
                    RegistrationId = table.Column<string>(nullable: false),
                    EMailAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.RegistrationId);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LogId = table.Column<Guid>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    MimeType = table.Column<string>(nullable: true),
                    RegistrationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Log_Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registration",
                        principalColumn: "RegistrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Log_RegistrationId",
                table: "Log",
                column: "RegistrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "Registration");
        }
    }
}
