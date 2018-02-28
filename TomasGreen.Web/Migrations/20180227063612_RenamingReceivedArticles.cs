using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class RenamingReceivedArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveArticleWarehouses_ReceiveArticles_ReceiveArticleID",
                table: "ReceiveArticleWarehouses");

            migrationBuilder.RenameColumn(
                name: "ReceiveArticleID",
                table: "ReceiveArticleWarehouses",
                newName: "PurchasedArticleID");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiveArticleWarehouses_ReceiveArticleID_WarehouseID",
                table: "ReceiveArticleWarehouses",
                newName: "IX_ReceiveArticleWarehouses_PurchasedArticleID_WarehouseID");

            migrationBuilder.AddColumn<long>(
                name: "ArticleUnit",
                table: "ReceiveArticles",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "ReceiveArticles",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PenaltyFee",
                table: "ReceiveArticles",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Received",
                table: "ReceiveArticles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TollFee",
                table: "ReceiveArticles",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "ReceiveArticles",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransportCost",
                table: "ReceiveArticles",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "ReceiveArticles",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveArticleWarehouses_ReceiveArticles_PurchasedArticleID",
                table: "ReceiveArticleWarehouses",
                column: "PurchasedArticleID",
                principalTable: "ReceiveArticles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveArticleWarehouses_ReceiveArticles_PurchasedArticleID",
                table: "ReceiveArticleWarehouses");

            migrationBuilder.DropColumn(
                name: "ArticleUnit",
                table: "ReceiveArticles");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ReceiveArticles");

            migrationBuilder.DropColumn(
                name: "PenaltyFee",
                table: "ReceiveArticles");

            migrationBuilder.DropColumn(
                name: "Received",
                table: "ReceiveArticles");

            migrationBuilder.DropColumn(
                name: "TollFee",
                table: "ReceiveArticles");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ReceiveArticles");

            migrationBuilder.DropColumn(
                name: "TransportCost",
                table: "ReceiveArticles");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "ReceiveArticles");

            migrationBuilder.RenameColumn(
                name: "PurchasedArticleID",
                table: "ReceiveArticleWarehouses",
                newName: "ReceiveArticleID");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiveArticleWarehouses_PurchasedArticleID_WarehouseID",
                table: "ReceiveArticleWarehouses",
                newName: "IX_ReceiveArticleWarehouses_ReceiveArticleID_WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveArticleWarehouses_ReceiveArticles_ReceiveArticleID",
                table: "ReceiveArticleWarehouses",
                column: "ReceiveArticleID",
                principalTable: "ReceiveArticles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
