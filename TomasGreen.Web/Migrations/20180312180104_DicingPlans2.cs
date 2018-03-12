using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class DicingPlans2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DicingPlans_CompanyID",
                table: "DicingPlans",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlans_ManagerID",
                table: "DicingPlans",
                column: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_DicingPlans_Companies_CompanyID",
                table: "DicingPlans",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DicingPlans_Employees_ManagerID",
                table: "DicingPlans",
                column: "ManagerID",
                principalTable: "Employees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DicingPlans_Companies_CompanyID",
                table: "DicingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_DicingPlans_Employees_ManagerID",
                table: "DicingPlans");

            migrationBuilder.DropIndex(
                name: "IX_DicingPlans_CompanyID",
                table: "DicingPlans");

            migrationBuilder.DropIndex(
                name: "IX_DicingPlans_ManagerID",
                table: "DicingPlans");
        }
    }
}
