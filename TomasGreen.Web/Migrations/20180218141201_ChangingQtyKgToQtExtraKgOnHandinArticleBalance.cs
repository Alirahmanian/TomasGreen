using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangingQtyKgToQtExtraKgOnHandinArticleBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QtyKgOnhand",
                table: "ArticleWarehouseBalances",
                newName: "QtyExtraKgOnhand");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QtyExtraKgOnhand",
                table: "ArticleWarehouseBalances",
                newName: "QtyKgOnhand");
        }
    }
}
