using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class orderCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyID",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "Orders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CurrencyID",
                table: "Orders",
                column: "CurrencyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Currencies_CurrencyID",
                table: "Orders",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Currencies_CurrencyID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CurrencyID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CurrencyID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Orders");
        }
    }
}
