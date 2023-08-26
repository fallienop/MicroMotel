using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroMotel.Services.Motel.Migrations
{
    /// <inheritdoc />
    public partial class addressloc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_Location",
                schema: "Motel",
                table: "Property",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Location",
                schema: "Motel",
                table: "Property");
        }
    }
}
