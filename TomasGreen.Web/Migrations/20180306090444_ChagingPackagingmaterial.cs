using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChagingPackagingmaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackagingMaterials_ArticleUnits_ArticleUnitID",
                table: "PackagingMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_PackagingMaterials_PackagingCategory_PackagingCategoryID1",
                table: "PackagingMaterials");

            migrationBuilder.DropTable(
                name: "PackagingCategory");

            migrationBuilder.DropIndex(
                name: "IX_PackagingMaterials_ArticleUnitID",
                table: "PackagingMaterials");

            migrationBuilder.DropIndex(
                name: "IX_PackagingMaterials_PackagingCategoryID1",
                table: "PackagingMaterials");

            migrationBuilder.DropColumn(
                name: "PackagingCategoryID1",
                table: "PackagingMaterials");

            migrationBuilder.AlterColumn<long>(
                name: "PackagingCategoryID",
                table: "PackagingMaterials",
                nullable: false,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "PackagingCategoryID",
                table: "PackagingMaterials",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "PackagingCategoryID1",
                table: "PackagingMaterials",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_PackagingMaterials_ArticleUnitID",
                table: "PackagingMaterials",
                column: "ArticleUnitID");

            migrationBuilder.CreateIndex(
                name: "IX_PackagingMaterials_PackagingCategoryID1",
                table: "PackagingMaterials",
                column: "PackagingCategoryID1");

            migrationBuilder.AddForeignKey(
                name: "FK_PackagingMaterials_ArticleUnits_ArticleUnitID",
                table: "PackagingMaterials",
                column: "ArticleUnitID",
                principalTable: "ArticleUnits",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackagingMaterials_PackagingCategory_PackagingCategoryID1",
                table: "PackagingMaterials",
                column: "PackagingCategoryID1",
                principalTable: "PackagingCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
