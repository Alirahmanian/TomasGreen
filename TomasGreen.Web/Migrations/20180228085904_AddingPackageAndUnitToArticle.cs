using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class AddingPackageAndUnitToArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticles_ArticlePakageForms_ArticlePackageFormID",
                table: "PurchasedArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticles_ArticleUnits_ArticleUnitID",
                table: "PurchasedArticles");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticles_ArticlePackageFormID",
                table: "PurchasedArticles");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticles_ArticleUnitID",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "ArticlePackageFormID",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "ArticleUnitID",
                table: "PurchasedArticles");

            migrationBuilder.RenameColumn(
                name: "QtyExtraKg",
                table: "PurchasedArticleWarehouses",
                newName: "QtyExtra");

            migrationBuilder.RenameColumn(
                name: "QtyBoxes",
                table: "PurchasedArticleWarehouses",
                newName: "QtyPackages");

            migrationBuilder.RenameColumn(
                name: "QtyKg",
                table: "OrderDetails",
                newName: "QtyExtra");

            migrationBuilder.RenameColumn(
                name: "QtyBoxes",
                table: "OrderDetails",
                newName: "QtyPackages");

            migrationBuilder.RenameColumn(
                name: "QtyExtraKgOut",
                table: "ArticleWarehouseBalances",
                newName: "QtyTotalOut");

            migrationBuilder.RenameColumn(
                name: "QtyExtraKgOnhand",
                table: "ArticleWarehouseBalances",
                newName: "QtyTotalOnhand");

            migrationBuilder.RenameColumn(
                name: "QtyExtraKgIn",
                table: "ArticleWarehouseBalances",
                newName: "QtyTotalIn");

            migrationBuilder.RenameColumn(
                name: "QtyBoxesOut",
                table: "ArticleWarehouseBalances",
                newName: "QtyPackagesOut");

            migrationBuilder.RenameColumn(
                name: "QtyBoxesOnhand",
                table: "ArticleWarehouseBalances",
                newName: "QtyPackagesOnhand");

            migrationBuilder.RenameColumn(
                name: "QtyBoxesIn",
                table: "ArticleWarehouseBalances",
                newName: "QtyPackagesIn");

            migrationBuilder.RenameColumn(
                name: "MinimumPrice",
                table: "Articles",
                newName: "MinimumPricePerUSD");

            migrationBuilder.AddColumn<decimal>(
                name: "QtyExtraIn",
                table: "ArticleWarehouseBalances",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QtyExtraOnhand",
                table: "ArticleWarehouseBalances",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QtyExtraOut",
                table: "ArticleWarehouseBalances",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "ArticlePackageFormID",
                table: "Articles",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ArticleUnitID",
                table: "Articles",
                nullable: false,
                defaultValue: 0L);

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ArticleUnits_ArticleUnitID",
                table: "Articles",
                column: "ArticleUnitID",
                principalTable: "ArticleUnits",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "QtyExtraIn",
                table: "ArticleWarehouseBalances");

            migrationBuilder.DropColumn(
                name: "QtyExtraOnhand",
                table: "ArticleWarehouseBalances");

            migrationBuilder.DropColumn(
                name: "QtyExtraOut",
                table: "ArticleWarehouseBalances");

            migrationBuilder.DropColumn(
                name: "ArticlePackageFormID",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ArticleUnitID",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "QtyPackages",
                table: "PurchasedArticleWarehouses",
                newName: "QtyBoxes");

            migrationBuilder.RenameColumn(
                name: "QtyExtra",
                table: "PurchasedArticleWarehouses",
                newName: "QtyExtraKg");

            migrationBuilder.RenameColumn(
                name: "QtyPackages",
                table: "OrderDetails",
                newName: "QtyBoxes");

            migrationBuilder.RenameColumn(
                name: "QtyExtra",
                table: "OrderDetails",
                newName: "QtyKg");

            migrationBuilder.RenameColumn(
                name: "QtyTotalOut",
                table: "ArticleWarehouseBalances",
                newName: "QtyExtraKgOut");

            migrationBuilder.RenameColumn(
                name: "QtyTotalOnhand",
                table: "ArticleWarehouseBalances",
                newName: "QtyExtraKgOnhand");

            migrationBuilder.RenameColumn(
                name: "QtyTotalIn",
                table: "ArticleWarehouseBalances",
                newName: "QtyExtraKgIn");

            migrationBuilder.RenameColumn(
                name: "QtyPackagesOut",
                table: "ArticleWarehouseBalances",
                newName: "QtyBoxesOut");

            migrationBuilder.RenameColumn(
                name: "QtyPackagesOnhand",
                table: "ArticleWarehouseBalances",
                newName: "QtyBoxesOnhand");

            migrationBuilder.RenameColumn(
                name: "QtyPackagesIn",
                table: "ArticleWarehouseBalances",
                newName: "QtyBoxesIn");

            migrationBuilder.RenameColumn(
                name: "MinimumPricePerUSD",
                table: "Articles",
                newName: "MinimumPrice");

            migrationBuilder.AddColumn<long>(
                name: "ArticlePackageFormID",
                table: "PurchasedArticles",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ArticleUnitID",
                table: "PurchasedArticles",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticles_ArticlePackageFormID",
                table: "PurchasedArticles",
                column: "ArticlePackageFormID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticles_ArticleUnitID",
                table: "PurchasedArticles",
                column: "ArticleUnitID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticles_ArticlePakageForms_ArticlePackageFormID",
                table: "PurchasedArticles",
                column: "ArticlePackageFormID",
                principalTable: "ArticlePakageForms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticles_ArticleUnits_ArticleUnitID",
                table: "PurchasedArticles",
                column: "ArticleUnitID",
                principalTable: "ArticleUnits",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
