using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class PackingPlanNewGUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PackingPlans",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PackingPlanMixs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PackingPlans");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PackingPlanMixs");
        }
    }
}
