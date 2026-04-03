using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Reservmed.Migrations
{
    /// <inheritdoc />
    public partial class ChangedRoleGUIDToManual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "005ff682-9c4f-49b4-bc04-2c3b1c457f2a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be716571-f1c0-4f51-987b-78df25266062");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d", null, "Doctor", "DOCTOR" },
                    { "f6e5d4c3-b2a1-0d9c-8b7a-6c5b4a3f2e1d", null, "Patient", "PATIENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6e5d4c3-b2a1-0d9c-8b7a-6c5b4a3f2e1d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "005ff682-9c4f-49b4-bc04-2c3b1c457f2a", null, "Patient", "PATIENT" },
                    { "be716571-f1c0-4f51-987b-78df25266062", null, "Doctor", "DOCTOR" }
                });
        }
    }
}
