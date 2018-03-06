using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChagingRoastingPlan20180306_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Articles_ArticleID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Warehouses_FromWarehouseID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Articles_NewArticleID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_PackagingMaterials_PackagingMaterialBagsID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_PackagingMaterials_PackagingMaterialPackagesID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Warehouses_ToWarehouseID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_FromWarehouseID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_NewArticleID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_PackagingMaterialBagsID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_PackagingMaterialPackagesID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_ToWarehouseID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "PackagingMaterialBagsID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "PackagingMaterialPackagesID",
                table: "RoastingPlans");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PackagingMaterialBagsID",
                table: "RoastingPlans",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PackagingMaterialPackagesID",
                table: "RoastingPlans",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_FromWarehouseID",
                table: "RoastingPlans",
                column: "FromWarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_NewArticleID",
                table: "RoastingPlans",
                column: "NewArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_PackagingMaterialBagsID",
                table: "RoastingPlans",
                column: "PackagingMaterialBagsID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_PackagingMaterialPackagesID",
                table: "RoastingPlans",
                column: "PackagingMaterialPackagesID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_ToWarehouseID",
                table: "RoastingPlans",
                column: "ToWarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Articles_ArticleID",
                table: "RoastingPlans",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Warehouses_FromWarehouseID",
                table: "RoastingPlans",
                column: "FromWarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Articles_NewArticleID",
                table: "RoastingPlans",
                column: "NewArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_PackagingMaterials_PackagingMaterialBagsID",
                table: "RoastingPlans",
                column: "PackagingMaterialBagsID",
                principalTable: "PackagingMaterials",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_PackagingMaterials_PackagingMaterialPackagesID",
                table: "RoastingPlans",
                column: "PackagingMaterialPackagesID",
                principalTable: "PackagingMaterials",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Warehouses_ToWarehouseID",
                table: "RoastingPlans",
                column: "ToWarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
