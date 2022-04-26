using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecoba.IdentityService.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Username = table.Column<string>(type: "text", nullable: false),
                    EmployeeId = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    GivenName = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    JobTitle = table.Column<string>(type: "text", nullable: true),
                    Mail = table.Column<string>(type: "text", nullable: false),
                    MobilePhone = table.Column<string>(type: "text", nullable: true),
                    OfficeLocation = table.Column<string>(type: "text", nullable: true),
                    PreferredLanguage = table.Column<string>(type: "text", nullable: true),
                    UserPrincipalName = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    PasswordSatl = table.Column<string>(type: "text", nullable: true),
                    IsResetPassword = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Editable = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Editable = table.Column<bool>(type: "boolean", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_User_Username",
                        column: x => x.Username,
                        principalTable: "User",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Username", "CreatedDate", "DisplayName", "Editable", "EmployeeId", "GivenName", "IsActive", "IsResetPassword", "JobTitle", "Mail", "MobilePhone", "OfficeLocation", "PasswordHash", "PasswordSatl", "PreferredLanguage", "Surname", "UpdatedDate", "UserPrincipalName" },
                values: new object[] { "admin", new DateTime(2022, 4, 19, 2, 24, 3, 216, DateTimeKind.Utc).AddTicks(5634), "Quản trị viên", false, "000", null, true, false, null, "it@ecoba.com.vn", null, null, "34bd87f0f70a6643578409ba1a931bf8", "b90cbd067696520d983458ea8402868c", null, null, new DateTime(2022, 4, 19, 2, 24, 3, 216, DateTimeKind.Utc).AddTicks(5636), null });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "Editable", "Role", "Username" },
                values: new object[] { 1, false, "Admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Username",
                table: "UserRole",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
