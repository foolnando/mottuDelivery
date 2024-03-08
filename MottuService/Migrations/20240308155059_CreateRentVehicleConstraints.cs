using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuService.Migrations
{
    /// <inheritdoc />
    public partial class CreateRentVehicleConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rents",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_DriverId",
                table: "Rents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rents",
                table: "Rents",
                columns: new[] { "DriverId", "VehicleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rents",
                table: "Rents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rents",
                table: "Rents",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_DriverId",
                table: "Rents",
                column: "DriverId");
        }
    }
}
