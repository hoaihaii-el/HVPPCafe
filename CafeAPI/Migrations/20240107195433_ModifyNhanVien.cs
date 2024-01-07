using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeAPI.Migrations
{
    public partial class ModifyNhanVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SoDienThoai",
                table: "NhanVien",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFullTime",
                table: "NhanVien",
                type: "bit",
                maxLength: 20,
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFullTime",
                table: "NhanVien");

            migrationBuilder.AlterColumn<string>(
                name: "SoDienThoai",
                table: "NhanVien",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
