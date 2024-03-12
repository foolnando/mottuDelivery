using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuService.Migrations
{
    /// <inheritdoc />
    public partial class RentIdOnOrderNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<Guid>(
                name: "RentId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "RentId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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
    }
}
