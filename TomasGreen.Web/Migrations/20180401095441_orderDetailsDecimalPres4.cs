using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class orderDetailsDecimalPres4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderID_ArticleID_WarehouseID_Price",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderDetails",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID_ArticleID_WarehouseID",
                table: "OrderDetails",
                columns: new[] { "OrderID", "ArticleID", "WarehouseID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderID_ArticleID_WarehouseID",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderDetails",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID_ArticleID_WarehouseID_Price",
                table: "OrderDetails",
                columns: new[] { "OrderID", "ArticleID", "WarehouseID", "Price" },
                unique: true);
        }
    }
}
