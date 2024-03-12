using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuService.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWithManyOrderConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RentDriverVehicleId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RentDriverVehicleId",
                table: "Orders",
                column: "RentDriverVehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Rents_RentDriverVehicleId",
                table: "Orders",
                column: "RentDriverVehicleId",
                principalTable: "Rents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Rents_RentDriverVehicleId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RentDriverVehicleId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RentDriverVehicleId",
                table: "Orders");
        }
    }
}
