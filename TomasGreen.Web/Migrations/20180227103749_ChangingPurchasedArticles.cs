using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangingPurchasedArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticles_ArticlePakageForms_ArticlePackageFormID",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "PackageFormID",
                table: "PurchasedArticles");

            migrationBuilder.AlterColumn<long>(
                name: "ArticlePackageFormID",
                table: "PurchasedArticles",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticles_ArticlePakageForms_ArticlePackageFormID",
                table: "PurchasedArticles",
                column: "ArticlePackageFormID",
                principalTable: "ArticlePakageForms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticles_ArticlePakageForms_ArticlePackageFormID",
                table: "PurchasedArticles");

            migrationBuilder.AlterColumn<long>(
                name: "ArticlePackageFormID",
                table: "PurchasedArticles",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<decimal>(
                name: "PackageFormID",
                table: "PurchasedArticles",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticles_ArticlePakageForms_ArticlePackageFormID",
                table: "PurchasedArticles",
                column: "ArticlePackageFormID",
                principalTable: "ArticlePakageForms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
