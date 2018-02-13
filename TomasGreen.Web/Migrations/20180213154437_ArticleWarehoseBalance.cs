using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ArticleWarehoseBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleWarehouseBalances",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<long>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    QtyBoxesIn = table.Column<int>(nullable: false),
                    QtyBoxesOut = table.Column<int>(nullable: false),
                    QtyBoxesReserved = table.Column<int>(nullable: false),
                    QtyExtraKgIn = table.Column<decimal>(nullable: false),
                    QtyExtraKgOut = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleWarehouseBalances", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArticleWarehouseBalances_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleWarehouseBalances_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalances_WarehouseID",
                table: "ArticleWarehouseBalances",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalances_ArticleID_WarehouseID",
                table: "ArticleWarehouseBalances",
                columns: new[] { "ArticleID", "WarehouseID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleWarehouseBalances");
        }
    }
}
