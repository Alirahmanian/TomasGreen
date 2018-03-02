using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ArrivedArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivedDate",
                table: "PurchasedArticleWarehouses",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QtyExtraArrived",
                table: "PurchasedArticleWarehouses",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "QtyPackagesArrived",
                table: "PurchasedArticleWarehouses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrivedDate",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.DropColumn(
                name: "QtyExtraArrived",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.DropColumn(
                name: "QtyPackagesArrived",
                table: "PurchasedArticleWarehouses");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Orders");
        }
    }
}
