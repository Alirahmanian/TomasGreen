using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class AddingCompanyIDToArticleWarehoseBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ArticleWarehouseBalances_ArticleID_WarehouseID",
                table: "ArticleWarehouseBalances");

            migrationBuilder.AddColumn<long>(
                name: "CompanyID",
                table: "ArticleWarehouseBalances",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalances_CompanyID",
                table: "ArticleWarehouseBalances",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalances_ArticleID_WarehouseID_CompanyID",
                table: "ArticleWarehouseBalances",
                columns: new[] { "ArticleID", "WarehouseID", "CompanyID" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleWarehouseBalances_Companies_CompanyID",
                table: "ArticleWarehouseBalances",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleWarehouseBalances_Companies_CompanyID",
                table: "ArticleWarehouseBalances");

            migrationBuilder.DropIndex(
                name: "IX_ArticleWarehouseBalances_CompanyID",
                table: "ArticleWarehouseBalances");

            migrationBuilder.DropIndex(
                name: "IX_ArticleWarehouseBalances_ArticleID_WarehouseID_CompanyID",
                table: "ArticleWarehouseBalances");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "ArticleWarehouseBalances");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalances_ArticleID_WarehouseID",
                table: "ArticleWarehouseBalances",
                columns: new[] { "ArticleID", "WarehouseID" },
                unique: true);
        }
    }
}
