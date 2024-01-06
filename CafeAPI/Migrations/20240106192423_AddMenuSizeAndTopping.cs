using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeAPI.Migrations
{
    public partial class AddMenuSizeAndTopping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiTietGia",
                columns: table => new
                {
                    MaMon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Size = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TyLeSizeM = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGia", x => new { x.MaMon, x.Size });
                    table.ForeignKey(
                        name: "FK_ChiTietGia_Mon_MaMon",
                        column: x => x.MaMon,
                        principalTable: "Mon",
                        principalColumn: "MaMon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Topping",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTopping = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topping", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietGia");

            migrationBuilder.DropTable(
                name: "Topping");
        }
    }
}
