using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class movingContainernrToPurchasedArticleHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainerNumber",
                table: "PurchasedArticleDetails");

            migrationBuilder.AddColumn<string>(
                name: "ContainerNumber",
                table: "PurchasedArticles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainerNumber",
                table: "PurchasedArticles");

            migrationBuilder.AddColumn<string>(
                name: "ContainerNumber",
                table: "PurchasedArticleDetails",
                nullable: true);
        }
    }
}
