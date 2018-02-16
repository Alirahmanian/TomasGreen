using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class Orderchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTranports_OrderTransportID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTranports_TransportIDID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TransportIDID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderID",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "TransportIDID",
                table: "Orders");

            migrationBuilder.AlterColumn<long>(
                name: "OrderTransportID",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders",
                column: "OrderTransportID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID_ArticleID",
                table: "OrderDetails",
                columns: new[] { "OrderID", "ArticleID" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTranports_OrderTransportID",
                table: "Orders",
                column: "OrderTransportID",
                principalTable: "OrderTranports",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTranports_OrderTransportID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrderID_ArticleID",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<long>(
                name: "OrderTransportID",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "TransportIDID",
                table: "Orders",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders",
                column: "OrderTransportID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TransportIDID",
                table: "Orders",
                column: "TransportIDID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID",
                table: "OrderDetails",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTranports_OrderTransportID",
                table: "Orders",
                column: "OrderTransportID",
                principalTable: "OrderTranports",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTranports_TransportIDID",
                table: "Orders",
                column: "TransportIDID",
                principalTable: "OrderTranports",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
