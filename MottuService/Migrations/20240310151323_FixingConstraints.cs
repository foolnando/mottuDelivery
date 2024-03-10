using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuService.Migrations
{
    /// <inheritdoc />
    public partial class FixingConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Rents_RentDriverId_RentVehicleId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderNotification_Drivers_DriverId",
                table: "OrderNotification");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderNotification_Order_OrderId",
                table: "OrderNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rents",
                table: "Rents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderNotification",
                table: "OrderNotification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_RentDriverId_RentVehicleId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "RentDriverId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "RentVehicleId",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "OrderNotification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_OrderNotification_OrderId",
                table: "Notifications",
                newName: "IX_Notifications_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderNotification_DriverId",
                table: "Notifications",
                newName: "IX_Notifications_DriverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rents",
                table: "Rents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_DriverId",
                table: "Rents",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RentId",
                table: "Orders",
                column: "RentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Drivers_DriverId",
                table: "Notifications",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Orders_OrderId",
                table: "Notifications",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Rents_RentId",
                table: "Orders",
                column: "RentId",
                principalTable: "Rents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Drivers_DriverId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Orders_OrderId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Rents_RentId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rents",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_DriverId",
                table: "Rents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RentId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "OrderNotification");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_OrderId",
                table: "OrderNotification",
                newName: "IX_OrderNotification_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_DriverId",
                table: "OrderNotification",
                newName: "IX_OrderNotification_DriverId");

            migrationBuilder.AddColumn<Guid>(
                name: "RentDriverId",
                table: "Order",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RentVehicleId",
                table: "Order",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rents",
                table: "Rents",
                columns: new[] { "DriverId", "VehicleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderNotification",
                table: "OrderNotification",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_RentDriverId_RentVehicleId",
                table: "Order",
                columns: new[] { "RentDriverId", "RentVehicleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Rents_RentDriverId_RentVehicleId",
                table: "Order",
                columns: new[] { "RentDriverId", "RentVehicleId" },
                principalTable: "Rents",
                principalColumns: new[] { "DriverId", "VehicleId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderNotification_Drivers_DriverId",
                table: "OrderNotification",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderNotification_Order_OrderId",
                table: "OrderNotification",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
