using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottuService.Migrations
{
    /// <inheritdoc />
    public partial class AlterRentTableColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Rents");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Rents",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<int>(
                name: "NumberDaysToRent",
                table: "Rents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberDaysToRent",
                table: "Rents");

            migrationBuilder.AlterColumn<float>(
                name: "Value",
                table: "Rents",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Rents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
