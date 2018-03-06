using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChagingPackagingmaterial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_PackagingMaterials_PackagingCategoryID",
                table: "PackagingMaterials",
                column: "PackagingCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_PackagingMaterials_ArticleUnits_ArticleUnitID",
                table: "PackagingMaterials",
                column: "ArticleUnitID",
                principalTable: "ArticleUnits",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PackagingMaterials_PackagingCategory_PackagingCategoryID",
                table: "PackagingMaterials",
                column: "PackagingCategoryID",
                principalTable: "PackagingCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackagingMaterials_ArticleUnits_ArticleUnitID",
                table: "PackagingMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_PackagingMaterials_PackagingCategory_PackagingCategoryID",
                table: "PackagingMaterials");

            migrationBuilder.DropTable(
                name: "PackagingCategory");

            migrationBuilder.DropIndex(
                name: "IX_PackagingMaterials_ArticleUnitID",
                table: "PackagingMaterials");

            migrationBuilder.DropIndex(
                name: "IX_PackagingMaterials_PackagingCategoryID",
                table: "PackagingMaterials");
        }
    }
}
