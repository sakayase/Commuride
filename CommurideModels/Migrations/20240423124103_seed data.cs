using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CommurideModels.Migrations
{
    /// <inheritdoc />
    public partial class seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Registration",
                value: "OK");

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Brand", "CO2", "Category", "Model", "Motorization", "NbPlaces", "Registration", "URLPhoto", "status" },
                values: new object[,]
                {
                    { 3, "renault", 21, 3, "megane", 0, 5, "OK", "", 0 },
                    { 4, "wolkswagen", 20, 2, "polo", 0, 5, "OK1", "", 1 },
                    { 5, "toyota", 3, 1, "yaris", 3, 5, "OK", "", 0 },
                    { 6, "tesla", 0, 5, "model c", 4, 2, "OK1", "", 0 },
                    { 7, "bmw", 24, 3, "serie a", 1, 5, "OK", "", 0 },
                    { 8, "renault", 20, 5, "laguna", 2, 2, "OK1", "", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Registration",
                value: "OK1");
        }
    }
}
