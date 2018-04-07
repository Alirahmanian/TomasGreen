using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class PurchasedArticleDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "PenaltyFee",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "Received",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "TollFee",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "TransportCost",
                table: "PurchasedArticles");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "WarehouseToWarehouses",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "WeightChange",
                table: "RoastingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWeight",
                table: "RoastingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "RoastingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "SaltLimit",
                table: "RoastingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Salt",
                table: "RoastingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "RoastingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerUnit",
                table: "RoastingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "NewTotalWeight",
                table: "RoastingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "NewQtyExtra",
                table: "RoastingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyID",
                table: "PurchasedArticles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtraArrived",
                table: "PurchasedArticleDetails",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "PurchasedArticleDetails",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "PackingPlanMixs",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerUnit",
                table: "PackingPlanMixs",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "NewTotalWeight",
                table: "PackingPlanMixs",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "NewQtyExtra",
                table: "PackingPlanMixs",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWeight",
                table: "PackingPlanMixArticles",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "PackingPlanMixArticles",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "MixPercent",
                table: "PackingPlanMixArticles",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Volume",
                table: "PackagingMaterials",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "PackagingMaterials",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Dimensions",
                table: "PackagingMaterials",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "OrderDetails",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWeight",
                table: "DicingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "DicingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "DicingPlans",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWeight",
                table: "DicingPlanDetails",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "DicingPlanDetails",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "DebitBeforeChange",
                table: "CompanyCreditDebitBalanceDetails",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditBeforeChange",
                table: "CompanyCreditDebitBalanceDetails",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SouldToUs",
                table: "Companies",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Received",
                table: "Companies",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Purchases",
                table: "Companies",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Paid",
                table: "Companies",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "LastBalance",
                table: "Companies",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Companies",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditReceived",
                table: "Companies",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditLimit",
                table: "Companies",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Companies",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtraOut",
                table: "ArticleWarehouseBalances",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtraOnhand",
                table: "ArticleWarehouseBalances",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtraIn",
                table: "ArticleWarehouseBalances",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "WeightPerPackage",
                table: "Articles",
                type: "decimal(18, 4)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.CreateTable(
                name: "PurchasedArticleCostTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedArticleCostTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedArticleCostDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    CurrencyID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    PurchaseArticleCostTypeID = table.Column<int>(nullable: false),
                    PurchasedArticleCostTypeID = table.Column<int>(nullable: true),
                    PurchasedArticleID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedArticleCostDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleCostDetails_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleCostDetails_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleCostDetails_PurchasedArticleCostTypes_PurchasedArticleCostTypeID",
                        column: x => x.PurchasedArticleCostTypeID,
                        principalTable: "PurchasedArticleCostTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchasedArticleCostDetails_PurchasedArticles_PurchasedArticleID",
                        column: x => x.PurchasedArticleID,
                        principalTable: "PurchasedArticles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleCostDetails_CompanyID",
                table: "PurchasedArticleCostDetails",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleCostDetails_CurrencyID",
                table: "PurchasedArticleCostDetails",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleCostTypeID",
                table: "PurchasedArticleCostDetails",
                column: "PurchasedArticleCostTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticleCostDetails_PurchasedArticleID",
                table: "PurchasedArticleCostDetails",
                column: "PurchasedArticleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasedArticleCostDetails");

            migrationBuilder.DropTable(
                name: "PurchasedArticleCostTypes");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "WarehouseToWarehouses",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WeightChange",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWeight",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SaltLimit",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Salt",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerUnit",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NewTotalWeight",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NewQtyExtra",
                table: "RoastingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyID",
                table: "PurchasedArticles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "PurchasedArticles",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PenaltyFee",
                table: "PurchasedArticles",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Received",
                table: "PurchasedArticles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TollFee",
                table: "PurchasedArticles",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransportCost",
                table: "PurchasedArticles",
                type: "decimal(18, 4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtraArrived",
                table: "PurchasedArticleDetails",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "PurchasedArticleDetails",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "PackingPlanMixs",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PricePerUnit",
                table: "PackingPlanMixs",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NewTotalWeight",
                table: "PackingPlanMixs",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "NewQtyExtra",
                table: "PackingPlanMixs",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWeight",
                table: "PackingPlanMixArticles",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "PackingPlanMixArticles",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MixPercent",
                table: "PackingPlanMixArticles",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Volume",
                table: "PackagingMaterials",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "PackagingMaterials",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Dimensions",
                table: "PackagingMaterials",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "OrderDetails",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWeight",
                table: "DicingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "DicingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "DicingPlans",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWeight",
                table: "DicingPlanDetails",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtra",
                table: "DicingPlanDetails",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DebitBeforeChange",
                table: "CompanyCreditDebitBalanceDetails",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditBeforeChange",
                table: "CompanyCreditDebitBalanceDetails",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SouldToUs",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Received",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Purchases",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Paid",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "LastBalance",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditReceived",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditLimit",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtraOut",
                table: "ArticleWarehouseBalances",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtraOnhand",
                table: "ArticleWarehouseBalances",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyExtraIn",
                table: "ArticleWarehouseBalances",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "WeightPerPackage",
                table: "Articles",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 4)");
        }
    }
}
