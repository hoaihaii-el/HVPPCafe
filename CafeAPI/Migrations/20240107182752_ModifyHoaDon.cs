using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeAPI.Migrations
{
    public partial class ModifyHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "HoaDon");

            migrationBuilder.AddColumn<bool>(
                name: "DaCheBien",
                table: "HoaDon",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DaThanhToan",
                table: "HoaDon",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaCheBien",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "DaThanhToan",
                table: "HoaDon");

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "HoaDon",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
