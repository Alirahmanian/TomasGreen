using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ArticleWwarehouseBalanceDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleWarehouseBalanceDetailTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UsedBySystem = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleWarehouseBalanceDetailTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ArticleWarehouseBalanceDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    ArticleID = table.Column<int>(nullable: false),
                    ArticleWarehouseBalanceDetailTypeID = table.Column<int>(nullable: false),
                    BalanceChangerID = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    CompanyID = table.Column<int>(nullable: false),
                    CurrencyID = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    QtyExtra = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    QtyExtraOnHandBeforeChange = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    QtyPackagesOnHandBeforeChange = table.Column<int>(nullable: false),
                    RowCreatedBySystem = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleWarehouseBalanceDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ArticleWarehouseBalanceDetails_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleWarehouseBalanceDetails_ArticleWarehouseBalanceDetailTypes_ArticleWarehouseBalanceDetailTypeID",
                        column: x => x.ArticleWarehouseBalanceDetailTypeID,
                        principalTable: "ArticleWarehouseBalanceDetailTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleWarehouseBalanceDetails_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleWarehouseBalanceDetails_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleWarehouseBalanceDetails_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalanceDetails_ArticleWarehouseBalanceDetailTypeID",
                table: "ArticleWarehouseBalanceDetails",
                column: "ArticleWarehouseBalanceDetailTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalanceDetails_CompanyID",
                table: "ArticleWarehouseBalanceDetails",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalanceDetails_CurrencyID",
                table: "ArticleWarehouseBalanceDetails",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalanceDetails_WarehouseID",
                table: "ArticleWarehouseBalanceDetails",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalanceDetails_ArticleID_WarehouseID_CompanyID_ArticleWarehouseBalanceDetailTypeID_BalanceChangerID",
                table: "ArticleWarehouseBalanceDetails",
                columns: new[] { "ArticleID", "WarehouseID", "CompanyID", "ArticleWarehouseBalanceDetailTypeID", "BalanceChangerID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleWarehouseBalanceDetailTypes_Name",
                table: "ArticleWarehouseBalanceDetailTypes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleWarehouseBalanceDetails");

            migrationBuilder.DropTable(
                name: "ArticleWarehouseBalanceDetailTypes");
        }
    }
}
