using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class PackingPlanChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerUnit",
                table: "PackingPlanMixs",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "PackingPlanMixs",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MixPercent",
                table: "PackingPlanMixArticles",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerUnit",
                table: "PackingPlanMixs");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "PackingPlanMixs");

            migrationBuilder.DropColumn(
                name: "MixPercent",
                table: "PackingPlanMixArticles");
        }
    }
}
