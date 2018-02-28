using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class RemoveingUnitAndPakageFromArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ArticlePakageForms_ArticlePackageFormID",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ArticleUnits_ArticleUnitID",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ArticlePackageFormID",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ArticleUnitID",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ArticlePackageFormID",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ArticleUnitID",
                table: "Articles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ArticlePackageFormID",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ArticleUnitID",
                table: "Articles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticlePackageFormID",
                table: "Articles",
                column: "ArticlePackageFormID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleUnitID",
                table: "Articles",
                column: "ArticleUnitID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ArticlePakageForms_ArticlePackageFormID",
                table: "Articles",
                column: "ArticlePackageFormID",
                principalTable: "ArticlePakageForms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ArticleUnits_ArticleUnitID",
                table: "Articles",
                column: "ArticleUnitID",
                principalTable: "ArticleUnits",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
