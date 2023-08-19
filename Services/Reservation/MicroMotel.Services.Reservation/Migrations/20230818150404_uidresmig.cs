using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroMotel.Services.Reservation.Migrations
{
    /// <inheritdoc />
    public partial class uidresmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                schema: "Reservation",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                schema: "Reservation",
                table: "Rooms");
        }
    }
}
