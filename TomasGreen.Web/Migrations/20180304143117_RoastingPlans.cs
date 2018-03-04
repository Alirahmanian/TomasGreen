using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class RoastingPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerRoastingPlans",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<long>(nullable: false),
                    ArticleUnitID = table.Column<int>(nullable: false),
                    CompanyID = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PricePerUnit = table.Column<decimal>(nullable: false),
                    QtExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    TotalWeight = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WeightChange = table.Column<decimal>(nullable: false),
                    WeightPerPackage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRoastingPlans", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomerRoastingPlans_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRoastingPlans_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoastingPlans",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<long>(nullable: false),
                    ArticleUnitID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    QtExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    TotalWeight = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WeightChange = table.Column<decimal>(nullable: false),
                    WeightPerPackage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoastingPlans", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoastingPlans_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoastingPlans_CompanyID",
                table: "CustomerRoastingPlans",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoastingPlans_ArticleID_CompanyID_Date",
                table: "CustomerRoastingPlans",
                columns: new[] { "ArticleID", "CompanyID", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_ArticleID_Date",
                table: "RoastingPlans",
                columns: new[] { "ArticleID", "Date" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerRoastingPlans");

            migrationBuilder.DropTable(
                name: "RoastingPlans");
        }
    }
}
