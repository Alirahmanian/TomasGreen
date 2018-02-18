using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class RenameingActiveToArchive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Warehouses",
                newName: "Archive");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "ReceiveArticles",
                newName: "Archive");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "OrderTranports",
                newName: "Archive");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Orders",
                newName: "Archive");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Employees",
                newName: "Archive");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Countries",
                newName: "Archive");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Companies",
                newName: "Archive");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Articles",
                newName: "Archive");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "ArticleCategories",
                newName: "Archive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Archive",
                table: "Warehouses",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Archive",
                table: "ReceiveArticles",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Archive",
                table: "OrderTranports",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Archive",
                table: "Orders",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Archive",
                table: "Employees",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Archive",
                table: "Countries",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Archive",
                table: "Companies",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Archive",
                table: "Articles",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Archive",
                table: "ArticleCategories",
                newName: "Active");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
