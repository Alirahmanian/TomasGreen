using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangeDatatypeForShortageDealingDescriptionProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PurchasedArticleShortageDealingDetails",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Description",
                table: "PurchasedArticleShortageDealingDetails",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
