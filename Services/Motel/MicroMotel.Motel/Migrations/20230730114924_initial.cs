using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroMotel.Services.Motel.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Motel");

            migrationBuilder.CreateTable(
                name: "Property",
                schema: "Motel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Building = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomCount = table.Column<byte>(type: "tinyint", nullable: false),
                    FloorCount = table.Column<byte>(type: "tinyint", nullable: false),
                    HasParking = table.Column<bool>(type: "bit", nullable: false),
                    HasOpenSpace = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meal",
                schema: "Motel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrepTime = table.Column<int>(type: "int", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meal_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "Motel",
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                schema: "Motel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BedCount = table.Column<byte>(type: "tinyint", nullable: false),
                    HasBath = table.Column<bool>(type: "bit", nullable: false),
                    HasFridge = table.Column<bool>(type: "bit", nullable: false),
                    HasTv = table.Column<bool>(type: "bit", nullable: false),
                    HasNetwork = table.Column<bool>(type: "bit", nullable: false),
                    HasAC = table.Column<bool>(type: "bit", nullable: false),
                    Floor = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "Motel",
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meal_PropertyId",
                schema: "Motel",
                table: "Meal",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_PropertyId",
                schema: "Motel",
                table: "Room",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meal",
                schema: "Motel");

            migrationBuilder.DropTable(
                name: "Room",
                schema: "Motel");

            migrationBuilder.DropTable(
                name: "Property",
                schema: "Motel");
        }
    }
}
