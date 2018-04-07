using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class RenameingPurchaseArticleCostType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID_PurchaseArticleCostTypeID_CompanyID_CurrencyID",
                table: "PurchasedArticleCostDetails");

            migrationBuilder.DropColumn(
                name: "PurchaseArticleCostTypeID",
                table: "PurchasedArticleCostDetails");

            migrationBuilder.AlterColumn<int>(
                name: "PurchasedArticleCostTypeID",
                table: "PurchasedArticleCostDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID_PurchasedArticleCostTypeID_CompanyID_CurrencyID",
                table: "PurchasedArticleCostDetails",
                columns: new[] { "PurchasedArticleID", "PurchasedArticleCostTypeID", "CompanyID", "CurrencyID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID_PurchasedArticleCostTypeID_CompanyID_CurrencyID",
                table: "PurchasedArticleCostDetails");

            migrationBuilder.AlterColumn<int>(
                name: "PurchasedArticleCostTypeID",
                table: "PurchasedArticleCostDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PurchaseArticleCostTypeID",
                table: "PurchasedArticleCostDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID_PurchaseArticleCostTypeID_CompanyID_CurrencyID",
                table: "PurchasedArticleCostDetails",
                columns: new[] { "PurchasedArticleID", "PurchaseArticleCostTypeID", "CompanyID", "CurrencyID" },
                unique: true);
        }
    }
}
