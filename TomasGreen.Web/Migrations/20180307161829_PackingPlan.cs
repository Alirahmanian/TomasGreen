using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class PackingPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackingPlans",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    CompanyID = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ManagerID = table.Column<long>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingPlans", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PackingPlanMixs",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Bags = table.Column<int>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    NewArticleID = table.Column<long>(nullable: false),
                    NewQtyExtra = table.Column<decimal>(nullable: false),
                    NewQtyPackages = table.Column<int>(nullable: false),
                    NewTotalWeight = table.Column<decimal>(nullable: false),
                    Packages = table.Column<int>(nullable: false),
                    PackagingMaterialBagID = table.Column<long>(nullable: false),
                    PackagingMaterialPackageID = table.Column<long>(nullable: false),
                    PackingPlanID = table.Column<long>(nullable: false),
                    ToWarehouseID = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingPlanMixs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PackingPlanMixs_PackingPlans_PackingPlanID",
                        column: x => x.PackingPlanID,
                        principalTable: "PackingPlans",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackingPlanMixArticles",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleID = table.Column<long>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PackingPlanMixID = table.Column<long>(nullable: false),
                    QtyExtra = table.Column<decimal>(nullable: false),
                    QtyPackages = table.Column<int>(nullable: false),
                    TotalWeight = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    WarehouseID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingPlanMixArticles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PackingPlanMixArticles_PackingPlanMixs_PackingPlanMixID",
                        column: x => x.PackingPlanMixID,
                        principalTable: "PackingPlanMixs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixArticles_PackingPlanMixID",
                table: "PackingPlanMixArticles",
                column: "PackingPlanMixID");

            migrationBuilder.CreateIndex(
                name: "IX_PackingPlanMixs_PackingPlanID",
                table: "PackingPlanMixs",
                column: "PackingPlanID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackingPlanMixArticles");

            migrationBuilder.DropTable(
                name: "PackingPlanMixs");

            migrationBuilder.DropTable(
                name: "PackingPlans");
        }
    }
}
