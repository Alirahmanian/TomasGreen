using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class AddingArchivetoAllEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "WarehouseToWarehouses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "RoastingPlans",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "PurchasedArticleDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "PaymentTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "PackingPlans",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "PackingPlanMixs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "PackingPlanMixArticles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "PackagingMaterials",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "PackagingCategory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "OrderDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "ExternalApis",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "ExternalApiFunctions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "DicingPlans",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "DicingPlanDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "CompanySections",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "CompanyCreditDebitBalances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "CompanyCreditDebitBalanceDetailTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "CompanyCreditDebitBalanceDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "ArticleWarehouseBalances",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "ArticleUnits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "ArticlePakageForms",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archive",
                table: "WarehouseToWarehouses");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "RoastingPlans");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "PurchasedArticleDetails");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "PackingPlans");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "PackingPlanMixs");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "PackingPlanMixArticles");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "PackagingMaterials");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "PackagingCategory");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "ExternalApis");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "ExternalApiFunctions");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "DicingPlans");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "DicingPlanDetails");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "CompanySections");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "CompanyCreditDebitBalances");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "CompanyCreditDebitBalanceDetailTypes");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "CompanyCreditDebitBalanceDetails");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "ArticleWarehouseBalances");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "ArticleUnits");

            migrationBuilder.DropColumn(
                name: "Archive",
                table: "ArticlePakageForms");
        }
    }
}
