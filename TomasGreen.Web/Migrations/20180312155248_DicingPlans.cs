using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class DicingPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DicingPlans",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<long>(nullable: false),
                    CompanyID = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: true),
                    ManagerID = table.Column<long>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicingPlans", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DicingPlanDetails",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<long>(nullable: false),
                    DicingPlanID = table.Column<long>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    TotalWeight = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DicingPlanDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DicingPlanDetails_Articles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DicingPlanDetails_DicingPlans_DicingPlanID",
                        column: x => x.DicingPlanID,
                        principalTable: "DicingPlans",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DicingPlanDetails_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlanDetails_ArticleID",
                table: "DicingPlanDetails",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlanDetails_WarehouseID",
                table: "DicingPlanDetails",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_DicingPlanDetails_DicingPlanID_WarehouseID_ArticleID",
                table: "DicingPlanDetails",
                columns: new[] { "DicingPlanID", "WarehouseID", "ArticleID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DicingPlanDetails");

            migrationBuilder.DropTable(
                name: "DicingPlans");
        }
    }
}
