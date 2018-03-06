using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChagingRoastingPlan20180306_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PackagingMaterialBoxID",
                table: "RoastingPlans",
                newName: "PackagingMaterialPackageID");

            migrationBuilder.RenameColumn(
                name: "Boxes",
                table: "RoastingPlans",
                newName: "Packages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PackagingMaterialPackageID",
                table: "RoastingPlans",
                newName: "PackagingMaterialBoxID");

            migrationBuilder.RenameColumn(
                name: "Packages",
                table: "RoastingPlans",
                newName: "Boxes");
        }
    }
}
