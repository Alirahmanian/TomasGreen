using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangestoRoastingPaln20180306 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Warehouses_WarehouseID",
                table: "RoastingPlans");

            migrationBuilder.AlterColumn<long>(
                name: "WarehouseID",
                table: "RoastingPlans",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "NewArticleUnitID",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<long>(
                name: "FromWarehouseID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ToWarehouseID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Warehouses_WarehouseID",
                table: "RoastingPlans",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Warehouses_WarehouseID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "FromWarehouseID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "ToWarehouseID",
                table: "RoastingPlans");

            migrationBuilder.AlterColumn<long>(
                name: "WarehouseID",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NewArticleUnitID",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Warehouses_WarehouseID",
                table: "RoastingPlans",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
