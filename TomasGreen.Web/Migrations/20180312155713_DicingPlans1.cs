using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class DicingPlans1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DicingPlans_ArticleID",
                table: "DicingPlans",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlans_WarehouseID",
                table: "DicingPlans",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_DicingPlans_Articles_ArticleID",
                table: "DicingPlans",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DicingPlans_Warehouses_WarehouseID",
                table: "DicingPlans",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DicingPlans_Articles_ArticleID",
                table: "DicingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_DicingPlans_Warehouses_WarehouseID",
                table: "DicingPlans");

            migrationBuilder.DropIndex(
                name: "IX_DicingPlans_ArticleID",
                table: "DicingPlans");

            migrationBuilder.DropIndex(
                name: "IX_DicingPlans_WarehouseID",
                table: "DicingPlans");
        }
    }
}
