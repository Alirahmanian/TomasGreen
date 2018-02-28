using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class AddingUniqueNameAndUnitToArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Name",
                table: "Articles");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Name_ArticleUnitID",
                table: "Articles",
                columns: new[] { "Name", "ArticleUnitID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Name_ArticleUnitID",
                table: "Articles");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Name",
                table: "Articles",
                column: "Name",
                unique: true);
        }
    }
}
