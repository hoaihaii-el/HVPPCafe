using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeAPI.Migrations
{
    public partial class CapNhatChamCong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietCaLam_LoaiCaLam_LoaiCaSoThuTu",
                table: "ChiTietCaLam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietCaLam",
                table: "ChiTietCaLam");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietCaLam_LoaiCaSoThuTu",
                table: "ChiTietCaLam");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietCaLam_MaNV",
                table: "ChiTietCaLam");

            migrationBuilder.DropColumn(
                name: "LoaiCaLam",
                table: "ChiTietCaLam");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "ChiTietCaLam");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "ChiTietCaLam");

            migrationBuilder.DropColumn(
                name: "LoaiCaSoThuTu",
                table: "ChiTietCaLam");

            migrationBuilder.AddColumn<double>(
                name: "SoGio",
                table: "ChiTietCaLam",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietCaLam",
                table: "ChiTietCaLam",
                columns: new[] { "MaNV", "Ngay" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietCaLam",
                table: "ChiTietCaLam");

            migrationBuilder.DropColumn(
                name: "SoGio",
                table: "ChiTietCaLam");

            migrationBuilder.AddColumn<int>(
                name: "LoaiCaLam",
                table: "ChiTietCaLam",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckIn",
                table: "ChiTietCaLam",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOut",
                table: "ChiTietCaLam",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LoaiCaSoThuTu",
                table: "ChiTietCaLam",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietCaLam",
                table: "ChiTietCaLam",
                columns: new[] { "LoaiCaLam", "MaNV", "Ngay" });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCaLam_LoaiCaSoThuTu",
                table: "ChiTietCaLam",
                column: "LoaiCaSoThuTu");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCaLam_MaNV",
                table: "ChiTietCaLam",
                column: "MaNV");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietCaLam_LoaiCaLam_LoaiCaSoThuTu",
                table: "ChiTietCaLam",
                column: "LoaiCaSoThuTu",
                principalTable: "LoaiCaLam",
                principalColumn: "SoThuTu");
        }
    }
}
