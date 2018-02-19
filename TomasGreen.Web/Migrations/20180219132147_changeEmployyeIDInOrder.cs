using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TomasGreen.Web.Migrations
{
    public partial class changeEmployyeIDInOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Employees_EmployeeID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "EmpoyeeID",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PaymentDate",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeID",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Employees_EmployeeID",
                table: "Orders",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Employees_EmployeeID",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PaymentDate",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeID",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "EmpoyeeID",
                table: "Orders",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Employees_EmployeeID",
                table: "Orders",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
