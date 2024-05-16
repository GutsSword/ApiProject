using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d189e4f-9888-446f-9d77-183a09d72b92");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88c72611-5f2d-4a4c-933e-b7d7792495f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c66c3469-68af-46d3-9cbb-faeda89402d9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8d9ad25c-c9d6-424d-a7cd-8a1feb254dfd", "73380b7f-9b7e-463a-bd02-9be6bb9c5d25", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a3632740-5810-43e1-8f05-9dd69b324fb0", "17c798f1-e17c-476a-b577-c5bbff3b49fd", "Editor", "EDITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "dc489a0e-756e-42a2-8bdf-4502c7c4a7a2", "3428811e-43a3-4bc4-a0a7-76cae8d892b7", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d9ad25c-c9d6-424d-a7cd-8a1feb254dfd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3632740-5810-43e1-8f05-9dd69b324fb0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc489a0e-756e-42a2-8bdf-4502c7c4a7a2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6d189e4f-9888-446f-9d77-183a09d72b92", "e5cd059c-736f-4a24-94dd-887e7d71987b", "Editor", "EDITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "88c72611-5f2d-4a4c-933e-b7d7792495f9", "4d900677-be27-4ab0-89aa-f0f11a3e279a", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c66c3469-68af-46d3-9cbb-faeda89402d9", "b6a6b63c-6d72-4da9-a83d-5b3f7e380f55", "Admin", "ADMIN" });
        }
    }
}
