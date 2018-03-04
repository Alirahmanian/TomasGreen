using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangesInRoastingPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerRoastingPlans");

            migrationBuilder.AddColumn<long>(
                name: "CompanyID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ManagerID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NewArticleID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "NewArticleUnitID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NewBags",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "NewPackageID",
                table: "RoastingPlans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewPackages",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "NewPackagingMaterialBagID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "NewPackagingMaterialPackageID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "NewQtyExtra",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "NewQtyPackages",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "NewTotalWeight",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NewWeightPerPackage",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "NewbagID",
                table: "RoastingPlans",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerUnit",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Salt",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SaltLimit",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "WarehouseID",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PackagingCategory",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackagingCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PackagingMaterial",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ArticleUnitID = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Dimensions = table.Column<decimal>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PackagingCategoryID = table.Column<bool>(nullable: false),
                    PackagingCategoryID1 = table.Column<long>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Volume = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackagingMaterial", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PackagingMaterial_ArticleUnits_ArticleUnitID",
                        column: x => x.ArticleUnitID,
                        principalTable: "ArticleUnits",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackagingMaterial_PackagingCategory_PackagingCategoryID1",
                        column: x => x.PackagingCategoryID1,
                        principalTable: "PackagingCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_CompanyID",
                table: "RoastingPlans",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_ManagerID",
                table: "RoastingPlans",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_NewPackageID",
                table: "RoastingPlans",
                column: "NewPackageID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_NewbagID",
                table: "RoastingPlans",
                column: "NewbagID");

            migrationBuilder.CreateIndex(
                name: "IX_RoastingPlans_WarehouseID",
                table: "RoastingPlans",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PackagingMaterial_ArticleUnitID",
                table: "PackagingMaterial",
                column: "ArticleUnitID");

            migrationBuilder.CreateIndex(
                name: "IX_PackagingMaterial_PackagingCategoryID1",
                table: "PackagingMaterial",
                column: "PackagingCategoryID1");

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Companies_CompanyID",
                table: "RoastingPlans",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Employees_ManagerID",
                table: "RoastingPlans",
                column: "ManagerID",
                principalTable: "Employees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_PackagingMaterial_NewPackageID",
                table: "RoastingPlans",
                column: "NewPackageID",
                principalTable: "PackagingMaterial",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_PackagingMaterial_NewbagID",
                table: "RoastingPlans",
                column: "NewbagID",
                principalTable: "PackagingMaterial",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoastingPlans_Warehouses_WarehouseID",
                table: "RoastingPlans",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Companies_CompanyID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Employees_ManagerID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_PackagingMaterial_NewPackageID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_PackagingMaterial_NewbagID",
                table: "RoastingPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_RoastingPlans_Warehouses_WarehouseID",
                table: "RoastingPlans");

            migrationBuilder.DropTable(
                name: "PackagingMaterial");

            migrationBuilder.DropTable(
                name: "PackagingCategory");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_CompanyID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_ManagerID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_NewPackageID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_NewbagID",
                table: "RoastingPlans");

            migrationBuilder.DropIndex(
                name: "IX_RoastingPlans_WarehouseID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "ManagerID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewArticleID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewArticleUnitID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewBags",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewPackageID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewPackages",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewPackagingMaterialBagID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewPackagingMaterialPackageID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewQtyExtra",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewQtyPackages",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewTotalWeight",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewWeightPerPackage",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "NewbagID",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "PricePerUnit",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "SaltLimit",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "WarehouseID",
                table: "RoastingPlans");

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

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoastingPlans_CompanyID",
                table: "CustomerRoastingPlans",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRoastingPlans_ArticleID_CompanyID_Date",
                table: "CustomerRoastingPlans",
                columns: new[] { "ArticleID", "CompanyID", "Date" },
                unique: true);
        }
    }
}
