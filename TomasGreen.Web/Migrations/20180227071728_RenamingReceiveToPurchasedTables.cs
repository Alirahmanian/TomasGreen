using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class RenamingReceiveToPurchasedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveArticles_Articles_ArticleID",
                table: "ReceiveArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveArticles_Companies_CompanyID",
                table: "ReceiveArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveArticleWarehouses_ReceiveArticles_PurchasedArticleID",
                table: "ReceiveArticleWarehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveArticleWarehouses_Warehouses_WarehouseID",
                table: "ReceiveArticleWarehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReceiveArticleWarehouses",
                table: "ReceiveArticleWarehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReceiveArticles",
                table: "ReceiveArticles");

            migrationBuilder.RenameTable(
                name: "ReceiveArticleWarehouses",
                newName: "PurchasedArticleWarehouses");

            migrationBuilder.RenameTable(
                name: "ReceiveArticles",
                newName: "PurchasedArticles");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiveArticleWarehouses_PurchasedArticleID_WarehouseID",
                table: "PurchasedArticleWarehouses",
                newName: "IX_PurchasedArticleWarehouses_PurchasedArticleID_WarehouseID");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiveArticleWarehouses_WarehouseID",
                table: "PurchasedArticleWarehouses",
                newName: "IX_PurchasedArticleWarehouses_WarehouseID");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiveArticles_CompanyID",
                table: "PurchasedArticles",
                newName: "IX_PurchasedArticles_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_ReceiveArticles_ArticleID",
                table: "PurchasedArticles",
                newName: "IX_PurchasedArticles_ArticleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchasedArticleWarehouses",
                table: "PurchasedArticleWarehouses",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchasedArticles",
                table: "PurchasedArticles",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticles_Articles_ArticleID",
                table: "PurchasedArticles",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticles_Companies_CompanyID",
                table: "PurchasedArticles",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticleWarehouses_PurchasedArticles_PurchasedArticleID",
                table: "PurchasedArticleWarehouses",
                column: "PurchasedArticleID",
                principalTable: "PurchasedArticles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticleWarehouses_Warehouses_WarehouseID",
                table: "PurchasedArticleWarehouses",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticles_Articles_ArticleID",
                table: "PurchasedArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticles_Companies_CompanyID",
                table: "PurchasedArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticleWarehouses_PurchasedArticles_PurchasedArticleID",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticleWarehouses_Warehouses_WarehouseID",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchasedArticleWarehouses",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchasedArticles",
                table: "PurchasedArticles");

            migrationBuilder.RenameTable(
                name: "PurchasedArticleWarehouses",
                newName: "ReceiveArticleWarehouses");

            migrationBuilder.RenameTable(
                name: "PurchasedArticles",
                newName: "ReceiveArticles");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedArticleWarehouses_PurchasedArticleID_WarehouseID",
                table: "ReceiveArticleWarehouses",
                newName: "IX_ReceiveArticleWarehouses_PurchasedArticleID_WarehouseID");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedArticleWarehouses_WarehouseID",
                table: "ReceiveArticleWarehouses",
                newName: "IX_ReceiveArticleWarehouses_WarehouseID");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedArticles_CompanyID",
                table: "ReceiveArticles",
                newName: "IX_ReceiveArticles_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_PurchasedArticles_ArticleID",
                table: "ReceiveArticles",
                newName: "IX_ReceiveArticles_ArticleID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReceiveArticleWarehouses",
                table: "ReceiveArticleWarehouses",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReceiveArticles",
                table: "ReceiveArticles",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveArticles_Articles_ArticleID",
                table: "ReceiveArticles",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveArticles_Companies_CompanyID",
                table: "ReceiveArticles",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveArticleWarehouses_ReceiveArticles_PurchasedArticleID",
                table: "ReceiveArticleWarehouses",
                column: "PurchasedArticleID",
                principalTable: "ReceiveArticles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveArticleWarehouses_Warehouses_WarehouseID",
                table: "ReceiveArticleWarehouses",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
