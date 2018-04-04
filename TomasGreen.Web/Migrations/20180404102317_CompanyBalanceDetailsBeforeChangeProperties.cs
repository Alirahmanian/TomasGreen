using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class CompanyBalanceDetailsBeforeChangeProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CreditBeforeChange",
                table: "CompanyBalanceDetails",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DebitBeforeChange",
                table: "CompanyBalanceDetails",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditBeforeChange",
                table: "CompanyBalanceDetails");

            migrationBuilder.DropColumn(
                name: "DebitBeforeChange",
                table: "CompanyBalanceDetails");
        }
    }
}
