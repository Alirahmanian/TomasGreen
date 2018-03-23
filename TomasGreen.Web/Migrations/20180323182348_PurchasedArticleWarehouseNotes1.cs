using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class PurchasedArticleWarehouseNotes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ArrivedAtWarehouseID",
                table: "PurchasedArticleWarehouses",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ArrivedAtWarehouseID",
                table: "PurchasedArticleWarehouses",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
