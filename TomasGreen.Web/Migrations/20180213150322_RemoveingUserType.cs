using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class RemoveingUserType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserType_DbTableName",
                table: "UserType");

            migrationBuilder.DropIndex(
                name: "IX_UserType_Name",
                table: "UserType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserType_DbTableName",
                table: "UserType",
                column: "DbTableName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserType_Name",
                table: "UserType",
                column: "Name",
                unique: true);
        }
    }
}
