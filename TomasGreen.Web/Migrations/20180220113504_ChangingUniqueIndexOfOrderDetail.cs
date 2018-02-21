using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangingUniqueIndexOfOrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderID_ArticleID",
                table: "OrderDetails");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID_ArticleID_WarehouseID_Price",
                table: "OrderDetails",
                columns: new[] { "OrderID", "ArticleID", "WarehouseID", "Price" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderID_ArticleID_WarehouseID_Price",
                table: "OrderDetails");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID_ArticleID",
                table: "OrderDetails",
                columns: new[] { "OrderID", "ArticleID" },
                unique: true);
        }
    }
}
