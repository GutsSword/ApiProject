using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    public partial class AddedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1caccad4-523c-4978-80ae-e400a09f4a83", "d8ccaa42-eca0-49a8-bfec-beeb7fc29881", "Editor", "EDITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "de140ffe-0bdc-4c5d-a461-9194d5d28b6f", "1838c0a8-02d6-4684-9aae-a3cb1c11979b", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e77b1cc8-7d50-4520-b53d-98e36b065b19", "1ccbbbda-34b2-4216-b94a-0f5f054ff171", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1caccad4-523c-4978-80ae-e400a09f4a83");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de140ffe-0bdc-4c5d-a461-9194d5d28b6f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e77b1cc8-7d50-4520-b53d-98e36b065b19");
        }
    }
}
