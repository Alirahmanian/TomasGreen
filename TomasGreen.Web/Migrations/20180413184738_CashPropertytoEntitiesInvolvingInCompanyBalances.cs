using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class CashPropertytoEntitiesInvolvingInCompanyBalances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cash",
                table: "PurchasedArticleShortageDealingDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cash",
                table: "PurchasedArticleCostDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cash",
                table: "CompanyCreditDebitBalances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Cash",
                table: "CompanyCreditDebitBalanceDetails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cash",
                table: "PurchasedArticleShortageDealingDetails");

            migrationBuilder.DropColumn(
                name: "Cash",
                table: "PurchasedArticleCostDetails");

            migrationBuilder.DropColumn(
                name: "Cash",
                table: "CompanyCreditDebitBalances");

            migrationBuilder.DropColumn(
                name: "Cash",
                table: "CompanyCreditDebitBalanceDetails");
        }
    }
}
