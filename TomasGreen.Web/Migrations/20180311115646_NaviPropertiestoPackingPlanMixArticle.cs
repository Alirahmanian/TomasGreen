using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class NaviPropertiestoPackingPlanMixArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixArticles_ArticleID",
                table: "PackingPlanMixArticles",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixArticles_WarehouseID",
                table: "PackingPlanMixArticles",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_PackingPlanMixArticles_Articles_ArticleID",
                table: "PackingPlanMixArticles",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackingPlanMixArticles_Warehouses_WarehouseID",
                table: "PackingPlanMixArticles",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackingPlanMixArticles_Articles_ArticleID",
                table: "PackingPlanMixArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_PackingPlanMixArticles_Warehouses_WarehouseID",
                table: "PackingPlanMixArticles");

            migrationBuilder.DropIndex(
                name: "IX_PackingPlanMixArticles_ArticleID",
                table: "PackingPlanMixArticles");

            migrationBuilder.DropIndex(
                name: "IX_PackingPlanMixArticles_WarehouseID",
                table: "PackingPlanMixArticles");
        }
    }
}
