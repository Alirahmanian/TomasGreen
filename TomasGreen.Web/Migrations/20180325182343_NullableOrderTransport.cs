using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class NullableOrderTransport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders",
                column: "OrderTransportID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders",
                column: "OrderTransportID",
                unique: true);
        }
    }
}
