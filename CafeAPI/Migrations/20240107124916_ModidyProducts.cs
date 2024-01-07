using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeAPI.Migrations
{
    public partial class ModidyProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_DonVi_DonViTenDonVi",
                table: "SanPham");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_NhomSanPham_NhomSanPhamTenNhom",
                table: "SanPham");

            migrationBuilder.DropIndex(
                name: "IX_SanPham_DonViTenDonVi",
                table: "SanPham");

            migrationBuilder.DropIndex(
                name: "IX_SanPham_NhomSanPhamTenNhom",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "DonViTenDonVi",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "NhomSanPhamTenNhom",
                table: "SanPham");

            migrationBuilder.RenameColumn(
                name: "TenNhomSanPham",
                table: "SanPham",
                newName: "Nhom");

            migrationBuilder.RenameColumn(
                name: "TenDonVi",
                table: "SanPham",
                newName: "DonVi");

            migrationBuilder.RenameColumn(
                name: "TenNhaCungCap",
                table: "ChiTietNhap",
                newName: "NhaCungCap");

            migrationBuilder.RenameColumn(
                name: "TenDonVi",
                table: "ChiTietNhap",
                newName: "DonVi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nhom",
                table: "SanPham",
                newName: "TenNhomSanPham");

            migrationBuilder.RenameColumn(
                name: "DonVi",
                table: "SanPham",
                newName: "TenDonVi");

            migrationBuilder.RenameColumn(
                name: "NhaCungCap",
                table: "ChiTietNhap",
                newName: "TenNhaCungCap");

            migrationBuilder.RenameColumn(
                name: "DonVi",
                table: "ChiTietNhap",
                newName: "TenDonVi");

            migrationBuilder.AddColumn<string>(
                name: "DonViTenDonVi",
                table: "SanPham",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NhomSanPhamTenNhom",
                table: "SanPham",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_DonViTenDonVi",
                table: "SanPham",
                column: "DonViTenDonVi");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_NhomSanPhamTenNhom",
                table: "SanPham",
                column: "NhomSanPhamTenNhom");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_DonVi_DonViTenDonVi",
                table: "SanPham",
                column: "DonViTenDonVi",
                principalTable: "DonVi",
                principalColumn: "TenDonVi");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_NhomSanPham_NhomSanPhamTenNhom",
                table: "SanPham",
                column: "NhomSanPhamTenNhom",
                principalTable: "NhomSanPham",
                principalColumn: "TenNhom");
        }
    }
}
