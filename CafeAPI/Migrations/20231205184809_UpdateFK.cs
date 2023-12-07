using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeAPI.Migrations
{
    public partial class UpdateFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoaiCaSoThuTu",
                table: "ChiTietCaLam",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCaLam_LoaiCaSoThuTu",
                table: "ChiTietCaLam",
                column: "LoaiCaSoThuTu");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietCaLam_LoaiCaLam_LoaiCaSoThuTu",
                table: "ChiTietCaLam",
                column: "LoaiCaSoThuTu",
                principalTable: "LoaiCaLam",
                principalColumn: "SoThuTu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietCaLam_LoaiCaLam_LoaiCaSoThuTu",
                table: "ChiTietCaLam");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietCaLam_LoaiCaSoThuTu",
                table: "ChiTietCaLam");

            migrationBuilder.DropColumn(
                name: "LoaiCaSoThuTu",
                table: "ChiTietCaLam");
        }
    }
}
