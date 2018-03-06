using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangestoRoastingPaln20180306_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Articles_ArticleID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Warehouses_WarehouseID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_WarehouseID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "WarehouseID",
                table: "RoastingPlans");

            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "Companies",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "Companies");

            migrationBuilder.AddColumn<long>(
                name: "WarehouseID",
                table: "RoastingPlans",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_WarehouseID",
                table: "RoastingPlans",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Articles_ArticleID",
                table: "RoastingPlans",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Warehouses_WarehouseID",
                table: "RoastingPlans",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
