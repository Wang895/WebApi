using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Migrations
{
    public partial class addemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("8293b240-b83c-4520-97e7-591ed3877592"), new Guid("2f9e2e78-96b1-4203-b4e9-87764f8ee601"), new DateTime(1994, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "光", 0, "李" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("8293b240-b83c-4520-97e7-591ed3877592"));
        }
    }
}
