using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangesmadeonRoastingPlanAndWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackagingMaterial_ArticleUnits_ArticleUnitID",
                table: "PackagingMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_PackagingMaterial_PackagingCategory_PackagingCategoryID1",
                table: "PackagingMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_PackagingMaterial_NewPackageID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_PackagingMaterial_NewbagID",
                table: "RoastingPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PackagingMaterial",
                table: "PackagingMaterial");

            migrationBuilder.RenameTable(
                name: "PackagingMaterial",
                newName: "PackagingMaterials");

            migrationBuilder.RenameIndex(
                name: "IX_PackagingMaterial_PackagingCategoryID1",
                table: "PackagingMaterials",
                newName: "IX_PackagingMaterials_PackagingCategoryID1");

            migrationBuilder.RenameIndex(
                name: "IX_PackagingMaterial_ArticleUnitID",
                table: "PackagingMaterials",
                newName: "IX_PackagingMaterials_ArticleUnitID");

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomers",
                table: "Warehouses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PackagingMaterials",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackagingMaterials",
                table: "PackagingMaterials",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_PackagingMaterials_Name_ArticleUnitID",
                table: "PackagingMaterials",
                columns: new[] { "Name", "ArticleUnitID" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PackagingMaterials_ArticleUnits_ArticleUnitID",
                table: "PackagingMaterials",
                column: "ArticleUnitID",
                principalTable: "ArticleUnits",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackagingMaterials_PackagingCategory_PackagingCategoryID1",
                table: "PackagingMaterials",
                column: "PackagingCategoryID1",
                principalTable: "PackagingCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackagingMaterials_ArticleUnits_ArticleUnitID",
                table: "PackagingMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_PackagingMaterials_PackagingCategory_PackagingCategoryID1",
                table: "PackagingMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_PackagingMaterials_NewPackageID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_PackagingMaterials_NewbagID",
                table: "RoastingPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PackagingMaterials",
                table: "PackagingMaterials");

            migrationBuilder.DropIndex(
                name: "IX_PackagingMaterials_Name_ArticleUnitID",
                table: "PackagingMaterials");

            migrationBuilder.DropColumn(
                name: "IsCustomers",
                table: "Warehouses");

            migrationBuilder.RenameTable(
                name: "PackagingMaterials",
                newName: "PackagingMaterial");

            migrationBuilder.RenameIndex(
                name: "IX_PackagingMaterials_PackagingCategoryID1",
                table: "PackagingMaterial",
                newName: "IX_PackagingMaterial_PackagingCategoryID1");

            migrationBuilder.RenameIndex(
                name: "IX_PackagingMaterials_ArticleUnitID",
                table: "PackagingMaterial",
                newName: "IX_PackagingMaterial_ArticleUnitID");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PackagingMaterial",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PackagingMaterial",
                table: "PackagingMaterial",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PackagingMaterial_ArticleUnits_ArticleUnitID",
                table: "PackagingMaterial",
                column: "ArticleUnitID",
                principalTable: "ArticleUnits",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackagingMaterial_PackagingCategory_PackagingCategoryID1",
                table: "PackagingMaterial",
                column: "PackagingCategoryID1",
                principalTable: "PackagingCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_PackagingMaterial_NewPackageID",
                table: "RoastingPlans",
                column: "NewPackageID",
                principalTable: "PackagingMaterial",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_PackagingMaterial_NewbagID",
                table: "RoastingPlans",
                column: "NewbagID",
                principalTable: "PackagingMaterial",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
