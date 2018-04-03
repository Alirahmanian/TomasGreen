using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class CompanyBalanceDetailIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyBalanceDetails_CompanyID_CurrencyID_CompanyBalanceDetailTypeID_BalanceChangerID",
                table: "CompanyBalanceDetails");

            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeID",
                table: "CompanyBalanceDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetails_CurrencyID",
                table: "CompanyBalanceDetails",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetails_PaymentTypeID",
                table: "CompanyBalanceDetails",
                column: "PaymentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetails_CompanyID_CurrencyID_CompanyBalanceDetailTypeID_BalanceChangerID_PaymentTypeID",
                table: "CompanyBalanceDetails",
                columns: new[] { "CompanyID", "CurrencyID", "CompanyBalanceDetailTypeID", "BalanceChangerID", "PaymentTypeID" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyBalanceDetails_Companies_CompanyID",
                table: "CompanyBalanceDetails",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyBalanceDetails_Currencies_CurrencyID",
                table: "CompanyBalanceDetails",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyBalanceDetails_PaymentTypes_PaymentTypeID",
                table: "CompanyBalanceDetails",
                column: "PaymentTypeID",
                principalTable: "PaymentTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyBalanceDetails_Companies_CompanyID",
                table: "CompanyBalanceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyBalanceDetails_Currencies_CurrencyID",
                table: "CompanyBalanceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyBalanceDetails_PaymentTypes_PaymentTypeID",
                table: "CompanyBalanceDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyBalanceDetails_CurrencyID",
                table: "CompanyBalanceDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyBalanceDetails_PaymentTypeID",
                table: "CompanyBalanceDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyBalanceDetails_CompanyID_CurrencyID_CompanyBalanceDetailTypeID_BalanceChangerID_PaymentTypeID",
                table: "CompanyBalanceDetails");

            migrationBuilder.DropColumn(
                name: "PaymentTypeID",
                table: "CompanyBalanceDetails");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyBalanceDetails_CompanyID_CurrencyID_CompanyBalanceDetailTypeID_BalanceChangerID",
                table: "CompanyBalanceDetails",
                columns: new[] { "CompanyID", "CurrencyID", "CompanyBalanceDetailTypeID", "BalanceChangerID" },
                unique: true);
        }
    }
}
