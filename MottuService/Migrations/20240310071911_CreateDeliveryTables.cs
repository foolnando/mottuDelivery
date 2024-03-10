using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuService.Migrations
{
    /// <inheritdoc />
    public partial class CreateDeliveryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    RentId = table.Column<Guid>(type: "uuid", nullable: false),
                    RentDriverId = table.Column<Guid>(type: "uuid", nullable: false),
                    RentVehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Rents_RentDriverId_RentVehicleId",
                        columns: x => new { x.RentDriverId, x.RentVehicleId },
                        principalTable: "Rents",
                        principalColumns: new[] { "DriverId", "VehicleId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderNotification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderNotification_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderNotification_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_RentDriverId_RentVehicleId",
                table: "Order",
                columns: new[] { "RentDriverId", "RentVehicleId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderNotification_DriverId",
                table: "OrderNotification",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderNotification_OrderId",
                table: "OrderNotification",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderNotification");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
