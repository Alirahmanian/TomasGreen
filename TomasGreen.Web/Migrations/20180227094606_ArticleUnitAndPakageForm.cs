using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ArticleUnitAndPakageForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArticleUnit",
                table: "PurchasedArticles",
                newName: "ArticleUnitID");

            migrationBuilder.RenameColumn(
                name: "BoxWeight",
                table: "Articles",
                newName: "WeightPerPackage");

            migrationBuilder.AddColumn<long>(
                name: "ArticlePackageFormID",
                table: "PurchasedArticles",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PackageFormID",
                table: "PurchasedArticles",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "ArticlePackageFormID",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ArticleUnitID",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MyProperty",
                table: "Articles",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ArticlePakageForms",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlePakageForms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ArticleUnits",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleUnits", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticles_ArticlePackageFormID",
                table: "PurchasedArticles",
                column: "ArticlePackageFormID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedArticles_ArticleUnitID",
                table: "PurchasedArticles",
                column: "ArticleUnitID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticlePackageFormID",
                table: "Articles",
                column: "ArticlePackageFormID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleUnitID",
                table: "Articles",
                column: "ArticleUnitID");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ArticlePakageForms_ArticlePackageFormID",
                table: "Articles",
                column: "ArticlePackageFormID",
                principalTable: "ArticlePakageForms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ArticleUnits_ArticleUnitID",
                table: "Articles",
                column: "ArticleUnitID",
                principalTable: "ArticleUnits",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticles_ArticlePakageForms_ArticlePackageFormID",
                table: "PurchasedArticles",
                column: "ArticlePackageFormID",
                principalTable: "ArticlePakageForms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedArticles_ArticleUnits_ArticleUnitID",
                table: "PurchasedArticles",
                column: "ArticleUnitID",
                principalTable: "ArticleUnits",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ArticlePakageForms_ArticlePackageFormID",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ArticleUnits_ArticleUnitID",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticles_ArticlePakageForms_ArticlePackageFormID",
                table: "PurchasedArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedArticles_ArticleUnits_ArticleUnitID",
                table: "PurchasedArticles");

            migrationBuilder.DropTable(
                name: "ArticlePakageForms");

            migrationBuilder.DropTable(
                name: "ArticleUnits");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticles_ArticlePackageFormID",
                table: "PurchasedArticles");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedArticles_ArticleUnitID",
                table: "PurchasedArticles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ArticlePackageFormID",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ArticleUnitID",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ArticlePackageFormID",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "PackageFormID",
                table: "PurchasedArticles");

            migrationBuilder.DropColumn(
                name: "ArticlePackageFormID",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ArticleUnitID",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "ArticleUnitID",
                table: "PurchasedArticles",
                newName: "ArticleUnit");

            migrationBuilder.RenameColumn(
                name: "WeightPerPackage",
                table: "Articles",
                newName: "BoxWeight");
        }
    }
}
