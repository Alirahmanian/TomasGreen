using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangesOnArticleBalanceAndOnthewayWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyTotalIn",
                table: "ArticleWarehouseBalances");

            migrationBuilder.DropColumn(
                name: "QtyTotalOnhand",
                table: "ArticleWarehouseBalances");

            migrationBuilder.DropColumn(
                name: "QtyTotalOut",
                table: "ArticleWarehouseBalances");

            migrationBuilder.RenameColumn(
                name: "IsReserve",
                table: "Warehouses",
                newName: "IsOnTheWay");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsOnTheWay",
                table: "Warehouses",
                newName: "IsReserve");

            migrationBuilder.AddColumn<decimal>(
                name: "QtyTotalIn",
                table: "ArticleWarehouseBalances",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QtyTotalOnhand",
                table: "ArticleWarehouseBalances",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QtyTotalOut",
                table: "ArticleWarehouseBalances",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
