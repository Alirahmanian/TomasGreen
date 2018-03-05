using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class AddingSectiontoWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanySectionID",
                table: "Warehouses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CompanySectionID",
                table: "Warehouses",
                column: "CompanySectionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouses_CompanySections_CompanySectionID",
                table: "Warehouses",
                column: "CompanySectionID",
                principalTable: "CompanySections",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouses_CompanySections_CompanySectionID",
                table: "Warehouses");

            migrationBuilder.DropIndex(
                name: "IX_Warehouses_CompanySectionID",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "CompanySectionID",
                table: "Warehouses");
        }
    }
}
