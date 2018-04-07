using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class IndexestoPurchasedArticleCostDetailAndType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID",
                table: "PurchasedArticleCostDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PurchasedArticleCostTypes",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleCostTypes_Name",
                table: "PurchasedArticleCostTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID_PurchaseArticleCostTypeID_CompanyID_CurrencyID",
                table: "PurchasedArticleCostDetails",
                columns: new[] { "PurchasedArticleID", "PurchaseArticleCostTypeID", "CompanyID", "CurrencyID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticleCostTypes_Name",
                table: "PurchasedArticleCostTypes");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID_PurchaseArticleCostTypeID_CompanyID_CurrencyID",
                table: "PurchasedArticleCostDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PurchasedArticleCostTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255);

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID",
                table: "PurchasedArticleCostDetails",
                column: "PurchasedArticleID");
        }
    }
}
