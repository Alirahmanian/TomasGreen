using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangingArticleUnit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeaasuresByPiece",
                table: "ArticleUnits");

            migrationBuilder.AddColumn<bool>(
                name: "MeasuresByPiece",
                table: "ArticleUnits",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasuresByPiece",
                table: "ArticleUnits");

            migrationBuilder.AddColumn<decimal>(
                name: "MeaasuresByPiece",
                table: "ArticleUnits",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
