using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class GuidToBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "WarehouseToWarehouses",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Warehouses",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "RoastingPlans",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PurchasedArticleCostTypes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PurchasedArticleContainerDetails",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PaymentTypes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PackingPlanMixArticles",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PackagingMaterials",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "PackagingCategory",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "OrderDetails",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "ExternalApis",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "ExternalApiFunctions",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "DicingPlanDetails",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Currencies",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CompanySections",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CompanyCreditDebitBalances",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CompanyCreditDebitBalanceDetailTypes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CompanyCreditDebitBalanceDetails",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "ArticleWarehouseBalances",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "ArticleWarehouseBalanceDetailTypes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "ArticleWarehouseBalanceDetails",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "ArticleUnits",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "ArticlePakageForms",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "ArticleCategories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "WarehouseToWarehouses");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PurchasedArticleCostTypes");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PurchasedArticleContainerDetails");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PackingPlanMixArticles");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PackagingMaterials");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "PackagingCategory");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "ExternalApis");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "ExternalApiFunctions");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "DicingPlanDetails");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CompanySections");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CompanyCreditDebitBalances");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CompanyCreditDebitBalanceDetailTypes");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CompanyCreditDebitBalanceDetails");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "ArticleWarehouseBalances");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "ArticleWarehouseBalanceDetailTypes");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "ArticleWarehouseBalanceDetails");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "ArticleUnits");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "ArticlePakageForms");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "ArticleCategories");
        }
    }
}
