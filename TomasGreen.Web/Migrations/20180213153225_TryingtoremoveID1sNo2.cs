using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class TryingtoremoveID1sNo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiveArticleWarehouses",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    QtyBoxes = table.Column<int>(nullable: false),
                    QtyExtraKg = table.Column<decimal>(nullable: false),
                    ReceiveArticleID = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveArticleWarehouses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReceiveArticleWarehouses_ReceiveArticles_ReceiveArticleID",
                        column: x => x.ReceiveArticleID,
                        principalTable: "ReceiveArticles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveArticleWarehouses_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveArticles_ArticleID",
                table: "ReceiveArticles",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveArticles_CompanyID",
                table: "ReceiveArticles",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveArticleWarehouses_WarehouseID",
                table: "ReceiveArticleWarehouses",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveArticleWarehouses_ReceiveArticleID_WarehouseID",
                table: "ReceiveArticleWarehouses",
                columns: new[] { "ReceiveArticleID", "WarehouseID" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveArticles_Articles_ArticleID",
                table: "ReceiveArticles",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveArticles_Companies_CompanyID",
                table: "ReceiveArticles",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveArticles_Articles_ArticleID",
                table: "ReceiveArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveArticles_Companies_CompanyID",
                table: "ReceiveArticles");

            migrationBuilder.DropTable(
                name: "ReceiveArticleWarehouses");

            migrationBuilder.DropIndex(
                name: "IX_ReceiveArticles_ArticleID",
                table: "ReceiveArticles");

            migrationBuilder.DropIndex(
                name: "IX_ReceiveArticles_CompanyID",
                table: "ReceiveArticles");
        }
    }
}
