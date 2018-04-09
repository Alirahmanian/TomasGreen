using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class PurchasedArticleShortageAndContainerDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainerNumber",
                table: "PurchasedArticles");

            migrationBuilder.CreateTable(
                name: "PurchasedArticleContainerDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    ContainerNumber = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PurchasedArticleID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedArticleContainerDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleContainerDetails_PurchasedArticles_PurchasedArticleID",
                        column: x => x.PurchasedArticleID,
                        principalTable: "PurchasedArticles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedArticleShortageDealingDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    CurrencyID = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PurchasedArticleID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedArticleShortageDealingDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleShortageDealingDetails_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleShortageDealingDetails_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleShortageDealingDetails_PurchasedArticles_PurchasedArticleID",
                        column: x => x.PurchasedArticleID,
                        principalTable: "PurchasedArticles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleContainerDetails_PurchasedArticleID_ContainerNumber",
                table: "PurchasedArticleContainerDetails",
                columns: new[] { "PurchasedArticleID", "ContainerNumber" },
                unique: true,
                filter: "[ContainerNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleShortageDealingDetails_CompanyID",
                table: "PurchasedArticleShortageDealingDetails",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleShortageDealingDetails_CurrencyID",
                table: "PurchasedArticleShortageDealingDetails",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleShortageDealingDetails_PurchasedArticleID_CompanyID_CurrencyID",
                table: "PurchasedArticleShortageDealingDetails",
                columns: new[] { "PurchasedArticleID", "CompanyID", "CurrencyID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasedArticleContainerDetails");

            migrationBuilder.DropTable(
                name: "PurchasedArticleShortageDealingDetails");

            migrationBuilder.AddColumn<string>(
                name: "ContainerNumber",
                table: "PurchasedArticles",
                nullable: true);
        }
    }
}
