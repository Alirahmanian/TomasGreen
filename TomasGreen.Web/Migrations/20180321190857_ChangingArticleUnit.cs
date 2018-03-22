using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangingArticleUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MeasurePerKg",
                table: "ArticleUnits",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleUnits_Name",
                table: "ArticleUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticlePakageForms_Name",
                table: "ArticlePakageForms",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ArticleUnits_Name",
                table: "ArticleUnits");

            migrationBuilder.DropIndex(
                name: "IX_ArticlePakageForms_Name",
                table: "ArticlePakageForms");

            migrationBuilder.DropColumn(
                name: "MeasurePerKg",
                table: "ArticleUnits");
        }
    }
}
