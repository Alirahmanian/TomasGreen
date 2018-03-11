using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class NewPropertiestoArticleUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MeasuresByG",
                table: "ArticleUnits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MeasuresByTon",
                table: "ArticleUnits",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasuresByG",
                table: "ArticleUnits");

            migrationBuilder.DropColumn(
                name: "MeasuresByTon",
                table: "ArticleUnits");
        }
    }
}
