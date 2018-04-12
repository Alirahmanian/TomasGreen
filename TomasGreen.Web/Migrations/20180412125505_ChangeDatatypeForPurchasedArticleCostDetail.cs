using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangeDatatypeForPurchasedArticleCostDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticleCostDetails_PurchasedArticleCostTypes_PurchasedArticleCostTypeID",
                table: "PurchasedArticleCostDetails");

            migrationBuilder.RenameColumn(
                name: "PurchasedArticleCostTypeID",
                table: "PurchasedArticleCostDetails",
                newName: "PaymentTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID_PurchasedArticleCostTypeID_CompanyID_CurrencyID",
                table: "PurchasedArticleCostDetails",
                newName: "IX_PurchasedArticleCostDetails_PurchasedArticleID_PaymentTypeID_CompanyID_CurrencyID");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleCostTypeID",
                table: "PurchasedArticleCostDetails",
                newName: "IX_PurchasedArticleCostDetails_PaymentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticleCostDetails_PaymentTypes_PaymentTypeID",
                table: "PurchasedArticleCostDetails",
                column: "PaymentTypeID",
                principalTable: "PaymentTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticleCostDetails_PaymentTypes_PaymentTypeID",
                table: "PurchasedArticleCostDetails");

            migrationBuilder.RenameColumn(
                name: "PaymentTypeID",
                table: "PurchasedArticleCostDetails",
                newName: "PurchasedArticleCostTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID_PaymentTypeID_CompanyID_CurrencyID",
                table: "PurchasedArticleCostDetails",
                newName: "IX_PurchasedArticleCostDetails_PurchasedArticleID_PurchasedArticleCostTypeID_CompanyID_CurrencyID");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedArticleCostDetails_PaymentTypeID",
                table: "PurchasedArticleCostDetails",
                newName: "IX_PurchasedArticleCostDetails_PurchasedArticleCostTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticleCostDetails_PurchasedArticleCostTypes_PurchasedArticleCostTypeID",
                table: "PurchasedArticleCostDetails",
                column: "PurchasedArticleCostTypeID",
                principalTable: "PurchasedArticleCostTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
