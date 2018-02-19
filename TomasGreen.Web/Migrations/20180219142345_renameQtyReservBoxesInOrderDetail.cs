using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class renameQtyReservBoxesInOrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QtyReservBoxes",
                table: "OrderDetails",
                newName: "QtyReserveBoxes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QtyReserveBoxes",
                table: "OrderDetails",
                newName: "QtyReservBoxes");
        }
    }
}
