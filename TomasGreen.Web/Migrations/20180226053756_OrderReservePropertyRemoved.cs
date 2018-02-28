using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class OrderReservePropertyRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyReserveBoxes",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "QtyBoxesReserved",
                table: "ArticleWarehouseBalances");

            migrationBuilder.AddColumn<bool>(
                name: "IsReserve",
                table: "Warehouses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReserve",
                table: "Warehouses");

            migrationBuilder.AddColumn<int>(
                name: "QtyReserveBoxes",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QtyBoxesReserved",
                table: "ArticleWarehouseBalances",
                nullable: false,
                defaultValue: 0);
        }
    }
}
