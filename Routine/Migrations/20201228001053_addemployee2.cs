using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Migrations
{
    public partial class addemployee2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("b71008b9-6659-44cb-88f5-2b6bf4a4872e"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("c83c219c-7a0d-4276-8167-dcf664fcb651"));

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[,]
                {
                    { new Guid("47008320-99f6-4064-87d9-1e0316c4c181"), new Guid("2f9e2e78-96b1-4203-b4e9-87764f8ee601"), new DateTime(1994, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "光", 1, "李" },
                    { new Guid("1c18c214-d921-4edf-8629-c5046eb4e67d"), new Guid("2f9e2e78-96b1-4203-b4e9-87764f8ee601"), new DateTime(1987, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "三", 1, "张" },
                    { new Guid("5896c321-b223-4509-bdd0-291199260cf1"), new Guid("5067c167-122d-464c-ac95-40322fae31d2"), new DateTime(1990, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "四", 2, "李" },
                    { new Guid("2149a84e-2cbe-428c-98e6-6856f15f1515"), new Guid("5067c167-122d-464c-ac95-40322fae31d2"), new DateTime(1990, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "4", "六", 2, "赵" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("1c18c214-d921-4edf-8629-c5046eb4e67d"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("2149a84e-2cbe-428c-98e6-6856f15f1515"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("47008320-99f6-4064-87d9-1e0316c4c181"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("5896c321-b223-4509-bdd0-291199260cf1"));

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("b71008b9-6659-44cb-88f5-2b6bf4a4872e"), new Guid("2f9e2e78-96b1-4203-b4e9-87764f8ee601"), new DateTime(1994, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "光", 0, "李" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("c83c219c-7a0d-4276-8167-dcf664fcb651"), new Guid("2f9e2e78-96b1-4203-b4e9-87764f8ee601"), new DateTime(1987, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "三", 0, "张" });
        }
    }
}
