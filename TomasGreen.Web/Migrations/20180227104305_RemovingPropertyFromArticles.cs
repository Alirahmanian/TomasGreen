using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class RemovingPropertyFromArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "MyProperty",
               table: "PurchasedArticles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
