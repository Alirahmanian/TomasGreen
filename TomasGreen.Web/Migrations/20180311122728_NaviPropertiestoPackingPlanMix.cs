using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class NaviPropertiestoPackingPlanMix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixs_NewArticleID",
                table: "PackingPlanMixs",
                column: "NewArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixs_ToWarehouseID",
                table: "PackingPlanMixs",
                column: "ToWarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_PackingPlanMixs_Articles_NewArticleID",
                table: "PackingPlanMixs",
                column: "NewArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PackingPlanMixs_Warehouses_ToWarehouseID",
                table: "PackingPlanMixs",
                column: "ToWarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackingPlanMixs_Articles_NewArticleID",
                table: "PackingPlanMixs");

            migrationBuilder.DropForeignKey(
                name: "FK_PackingPlanMixs_Warehouses_ToWarehouseID",
                table: "PackingPlanMixs");

            migrationBuilder.DropIndex(
                name: "IX_PackingPlanMixs_NewArticleID",
                table: "PackingPlanMixs");

            migrationBuilder.DropIndex(
                name: "IX_PackingPlanMixs_ToWarehouseID",
                table: "PackingPlanMixs");
        }
    }
}
