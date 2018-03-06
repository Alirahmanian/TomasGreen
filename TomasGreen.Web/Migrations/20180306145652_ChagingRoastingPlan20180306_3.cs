using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChagingRoastingPlan20180306_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bags",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Boxes",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PackagingMaterialBagID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PackagingMaterialBoxID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bags",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "Boxes",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "PackagingMaterialBagID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "PackagingMaterialBoxID",
                table: "RoastingPlans");
        }
    }
}
