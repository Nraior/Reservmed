using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Reservmed.Migrations
{
    /// <inheritdoc />
    public partial class IdentityRoleSeedingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "081b4b4c-6339-47d6-b0c7-e5cf15418d26");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d76c9622-4270-4f44-91a5-52b4e536682c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "005ff682-9c4f-49b4-bc04-2c3b1c457f2a", null, "Patient", "PATIENT" },
                    { "be716571-f1c0-4f51-987b-78df25266062", null, "Doctor", "DOCTOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "081b4b4c-6339-47d6-b0c7-e5cf15418d26", null, "Doctor", "DOCTOR" },
                    { "d76c9622-4270-4f44-91a5-52b4e536682c", null, "Patient", "Patient" }
                });
        }
    }
}
