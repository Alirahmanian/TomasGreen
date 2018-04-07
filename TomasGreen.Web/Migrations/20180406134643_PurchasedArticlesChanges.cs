using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class PurchasedArticlesChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticles_Articles_ArticleID",
                table: "PurchasedArticles");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticles_ArticleID",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "ArticleID",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "ContainerNumber",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "TotalPerUnit",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "PurchasedArticles");

            migrationBuilder.AddColumn<string>(
                name: "ContainerNumber",
                table: "PurchasedArticleWarehouses",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPerUnit",
                table: "PurchasedArticleWarehouses",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "PurchasedArticleWarehouses",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyID",
                table: "PurchasedArticles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleWarehouses_ArrivedAtWarehouseID",
                table: "PurchasedArticleWarehouses",
                column: "ArrivedAtWarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticleWarehouses_Warehouses_ArrivedAtWarehouseID",
                table: "PurchasedArticleWarehouses",
                column: "ArrivedAtWarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticleWarehouses_Warehouses_ArrivedAtWarehouseID",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticleWarehouses_ArrivedAtWarehouseID",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.DropColumn(
                name: "ContainerNumber",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.DropColumn(
                name: "TotalPerUnit",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyID",
                table: "PurchasedArticles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ArticleID",
                table: "PurchasedArticles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContainerNumber",
                table: "PurchasedArticles",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPerUnit",
                table: "PurchasedArticles",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "PurchasedArticles",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticles_ArticleID",
                table: "PurchasedArticles",
                column: "ArticleID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticles_Articles_ArticleID",
                table: "PurchasedArticles",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
