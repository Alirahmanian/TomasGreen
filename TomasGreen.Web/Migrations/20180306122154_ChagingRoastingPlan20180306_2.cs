using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChagingRoastingPlan20180306_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QtExtra",
                table: "RoastingPlans",
                newName: "QtyExtra");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QtyExtra",
                table: "RoastingPlans",
                newName: "QtExtra");
        }
    }
}
