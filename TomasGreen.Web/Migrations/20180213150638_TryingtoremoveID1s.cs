using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class TryingtoremoveID1s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SystemUser_SystemUserIdID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SystemUser");

            migrationBuilder.DropTable(
                name: "UserType");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SystemUserIdID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SystemUserIdID",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SystemUserIdID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    DbTableName = table.Column<string>(maxLength: 255, nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SystemUser",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    CompanyID = table.Column<int>(nullable: true),
                    CompanyID1 = table.Column<long>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    UserTypeID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUser", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SystemUser_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SystemUser_Companies_CompanyID1",
                        column: x => x.CompanyID1,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SystemUser_UserType_UserTypeID",
                        column: x => x.UserTypeID,
                        principalTable: "UserType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SystemUserIdID",
                table: "AspNetUsers",
                column: "SystemUserIdID");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUser_ApplicationUserId",
                table: "SystemUser",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUser_CompanyID1",
                table: "SystemUser",
                column: "CompanyID1");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUser_UserTypeID",
                table: "SystemUser",
                column: "UserTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SystemUser_SystemUserIdID",
                table: "AspNetUsers",
                column: "SystemUserIdID",
                principalTable: "SystemUser",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
