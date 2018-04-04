using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class boolPropertiestoBalanceDetailAndPaymentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UsedBySystem",
                table: "PaymentTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsedBySystem",
                table: "CompanyCreditDebitBalanceDetailTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RowCreatedBySystem",
                table: "CompanyCreditDebitBalanceDetails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsedBySystem",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "UsedBySystem",
                table: "CompanyCreditDebitBalanceDetailTypes");

            migrationBuilder.DropColumn(
                name: "RowCreatedBySystem",
                table: "CompanyCreditDebitBalanceDetails");
        }
    }
}
