using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChagingRoastingPlan20180306_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_PackagingMaterials_NewPackageID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_PackagingMaterials_NewbagID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_NewPackageID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_NewbagID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "ArticleUnitID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewArticleUnitID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewBags",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewPackageID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewPackages",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewPackagingMaterialBagID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewPackagingMaterialPackageID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewWeightPerPackage",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewbagID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "WeightPerPackage",
                table: "RoastingPlans");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleUnitID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "NewArticleUnitID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "NewBags",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "NewPackageID",
                table: "RoastingPlans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewPackages",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "NewPackagingMaterialBagID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NewPackagingMaterialPackageID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "NewWeightPerPackage",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "NewbagID",
                table: "RoastingPlans",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightPerPackage",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_NewPackageID",
                table: "RoastingPlans",
                column: "NewPackageID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_NewbagID",
                table: "RoastingPlans",
                column: "NewbagID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_PackagingMaterials_NewPackageID",
                table: "RoastingPlans",
                column: "NewPackageID",
                principalTable: "PackagingMaterials",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_PackagingMaterials_NewbagID",
                table: "RoastingPlans",
                column: "NewbagID",
                principalTable: "PackagingMaterials",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
