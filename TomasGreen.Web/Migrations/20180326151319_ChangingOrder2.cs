using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class ChangingOrder2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTranports_OrderTransportID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderTranports");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AmountArticle",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AmountReserve",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ForcedPaid",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LoadingDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderTransportID",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "TransportFee",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransportFee",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "AmountArticle",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmountReserve",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ForcedPaid",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LoadingDate",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderTransportID",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderTranports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    Archive = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTranports", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTransportID",
                table: "Orders",
                column: "OrderTransportID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTranports_Name",
                table: "OrderTranports",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTranports_OrderTransportID",
                table: "Orders",
                column: "OrderTransportID",
                principalTable: "OrderTranports",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
