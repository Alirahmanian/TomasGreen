using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class DescriptionPropertyToPurchasedArticleCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleWarehouseBalanceDetails_Currencies_CurrencyID",
                table: "ArticleWarehouseBalanceDetails");

            migrationBuilder.DropIndex(
                name: "IX_ArticleWarehouseBalanceDetails_CurrencyID",
                table: "ArticleWarehouseBalanceDetails");

            migrationBuilder.DropColumn(
                name: "CurrencyID",
                table: "ArticleWarehouseBalanceDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "Description",
                table: "PurchasedArticleShortageDealingDetails",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PurchasedArticleShortageDealingDetails");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyID",
                table: "ArticleWarehouseBalanceDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalanceDetails_CurrencyID",
                table: "ArticleWarehouseBalanceDetails",
                column: "CurrencyID");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleWarehouseBalanceDetails_Currencies_CurrencyID",
                table: "ArticleWarehouseBalanceDetails",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
