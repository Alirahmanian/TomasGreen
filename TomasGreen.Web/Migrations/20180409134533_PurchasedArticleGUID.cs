using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class PurchasedArticleGUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PurchasedArticleShortageDealingDetails",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PurchasedArticleDetails",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PurchasedArticleCostDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PurchasedArticleShortageDealingDetails");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PurchasedArticleDetails");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PurchasedArticleCostDetails");
        }
    }
}
