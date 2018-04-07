using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class renamingPurchasedArticleWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasedArticleWarehouses");

            migrationBuilder.CreateTable(
                name: "PurchasedArticleDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArrivedAtWarehouseID = table.Column<int>(nullable: true),
                    ArrivedDate = table.Column<DateTime>(nullable: true),
                    ArticleID = table.Column<int>(nullable: false),
                    ContainerNumber = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    PurchasedArticleID = table.Column<int>(nullable: false),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyExtraArrived = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    QtyPackagesArrived = table.Column<int>(nullable: false),
                    TotalPerUnit = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedArticleDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleDetails_Warehouses_ArrivedAtWarehouseID",
                        column: x => x.ArrivedAtWarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleDetails_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleDetails_PurchasedArticles_PurchasedArticleID",
                        column: x => x.PurchasedArticleID,
                        principalTable: "PurchasedArticles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleDetails_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleDetails_ArrivedAtWarehouseID",
                table: "PurchasedArticleDetails",
                column: "ArrivedAtWarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleDetails_ArticleID",
                table: "PurchasedArticleDetails",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleDetails_WarehouseID",
                table: "PurchasedArticleDetails",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleDetails_PurchasedArticleID_WarehouseID",
                table: "PurchasedArticleDetails",
                columns: new[] { "PurchasedArticleID", "WarehouseID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasedArticleDetails");

            migrationBuilder.CreateTable(
                name: "PurchasedArticleWarehouses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArrivedAtWarehouseID = table.Column<int>(nullable: true),
                    ArrivedDate = table.Column<DateTime>(nullable: true),
                    ContainerNumber = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    PurchasedArticleID = table.Column<int>(nullable: false),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyExtraArrived = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    QtyPackagesArrived = table.Column<int>(nullable: false),
                    TotalPerUnit = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedArticleWarehouses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleWarehouses_Warehouses_ArrivedAtWarehouseID",
                        column: x => x.ArrivedAtWarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleWarehouses_PurchasedArticles_PurchasedArticleID",
                        column: x => x.PurchasedArticleID,
                        principalTable: "PurchasedArticles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleWarehouses_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleWarehouses_ArrivedAtWarehouseID",
                table: "PurchasedArticleWarehouses",
                column: "ArrivedAtWarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleWarehouses_WarehouseID",
                table: "PurchasedArticleWarehouses",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleWarehouses_PurchasedArticleID_WarehouseID",
                table: "PurchasedArticleWarehouses",
                columns: new[] { "PurchasedArticleID", "WarehouseID" },
                unique: true);
        }
    }
}
