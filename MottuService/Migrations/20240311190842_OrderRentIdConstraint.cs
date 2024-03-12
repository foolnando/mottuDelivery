using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuService.Migrations
{
    /// <inheritdoc />
    public partial class OrderRentIdConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Rents_RentId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Rents_RentId",
                table: "Orders",
                column: "RentId",
                principalTable: "Rents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Rents_RentId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Rents_RentId",
                table: "Orders",
                column: "RentId",
                principalTable: "Rents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
