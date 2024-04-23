using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CommurideModels.Migrations
{
    /// <inheritdoc />
    public partial class seeddataanddeleteuserfromvehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_AspNetUsers_userId",
                table: "Rents");

            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Vehicles_vehicleId",
                table: "Rents");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_userId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_userId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "AdressArrival",
                table: "Rents");

            migrationBuilder.RenameColumn(
                name: "vehicleId",
                table: "Rents",
                newName: "VehicleId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Rents",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "DateHourReturn",
                table: "Rents",
                newName: "DateHourStart");

            migrationBuilder.RenameColumn(
                name: "DateHourLeaving",
                table: "Rents",
                newName: "DateHourEnd");

            migrationBuilder.RenameIndex(
                name: "IX_Rents_vehicleId",
                table: "Rents",
                newName: "IX_Rents_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Rents_userId",
                table: "Rents",
                newName: "IX_Rents_UserId");

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Brand", "CO2", "Category", "Model", "Motorization", "NbPlaces", "Registration", "URLPhoto", "status" },
                values: new object[,]
                {
                    { 1, "renault", 11, 1, "clio", 3, 5, "OK", "", 0 },
                    { 2, "mazda", 20, 3, "MX5", 1, 2, "OK1", "", 0 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_AspNetUsers_UserId",
                table: "Rents",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Vehicles_VehicleId",
                table: "Rents",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_AspNetUsers_UserId",
                table: "Rents");

            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Vehicles_VehicleId",
                table: "Rents");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "Rents",
                newName: "vehicleId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Rents",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "DateHourStart",
                table: "Rents",
                newName: "DateHourReturn");

            migrationBuilder.RenameColumn(
                name: "DateHourEnd",
                table: "Rents",
                newName: "DateHourLeaving");

            migrationBuilder.RenameIndex(
                name: "IX_Rents_VehicleId",
                table: "Rents",
                newName: "IX_Rents_vehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Rents_UserId",
                table: "Rents",
                newName: "IX_Rents_userId");

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Vehicles",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AdressArrival",
                table: "Rents",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_userId",
                table: "Vehicles",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_AspNetUsers_userId",
                table: "Rents",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Vehicles_vehicleId",
                table: "Rents",
                column: "vehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_userId",
                table: "Vehicles",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
