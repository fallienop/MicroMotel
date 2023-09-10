using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroMotel.Services.Reservation.Migrations
{
    /// <inheritdoc />
    public partial class roomrtotalprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                schema: "Reservation",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "Reservation",
                table: "Rooms");
        }
    }
}
