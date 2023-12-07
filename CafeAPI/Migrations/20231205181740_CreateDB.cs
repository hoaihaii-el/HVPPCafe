using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeAPI.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ban",
                columns: table => new
                {
                    SoBan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ban", x => x.SoBan);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietNhap",
                columns: table => new
                {
                    MaNhap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSanPham = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuong = table.Column<float>(type: "real", nullable: false),
                    GiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TenDonVi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietNhap", x => x.MaNhap);
                });

            migrationBuilder.CreateTable(
                name: "ChucVu",
                columns: table => new
                {
                    TenChucVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Part_time = table.Column<bool>(type: "bit", nullable: false),
                    MucLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucVu", x => x.TenChucVu);
                });

            migrationBuilder.CreateTable(
                name: "DonVi",
                columns: table => new
                {
                    TenDonVi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonVi", x => x.TenDonVi);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MucApDung = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Xoa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai", x => x.MaKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "LoaiCaLam",
                columns: table => new
                {
                    SoThuTu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GioBatDau = table.Column<float>(type: "real", nullable: false),
                    GioKetThuc = table.Column<float>(type: "real", nullable: false),
                    HeSoLuong = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiCaLam", x => x.SoThuTu);
                });

            migrationBuilder.CreateTable(
                name: "Mon",
                columns: table => new
                {
                    MaMon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenMon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AnhMonAn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GiaBan = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mon", x => x.MaMon);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    TenNhaCungCap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LienLac = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCap", x => x.TenNhaCungCap);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    MaNV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChucVu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NgayVaoLam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Xoa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "NhomSanPham",
                columns: table => new
                {
                    TenNhom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhomSanPham", x => x.TenNhom);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietCaLam",
                columns: table => new
                {
                    LoaiCaLam = table.Column<int>(type: "int", nullable: false),
                    MaNV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ngay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietCaLam", x => new { x.LoaiCaLam, x.MaNV, x.Ngay });
                    table.ForeignKey(
                        name: "FK_ChiTietCaLam_NhanVien_MaNV",
                        column: x => x.MaNV,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    SoHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiHoaDon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TriGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayHoaDon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SoBan = table.Column<int>(type: "int", nullable: false),
                    KhuyenMaiMaKhuyenMai = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    NhanVienMaNV = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.SoHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDon_KhuyenMai_KhuyenMaiMaKhuyenMai",
                        column: x => x.KhuyenMaiMaKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "MaKhuyenMai");
                    table.ForeignKey(
                        name: "FK_HoaDon_NhanVien_NhanVienMaNV",
                        column: x => x.NhanVienMaNV,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Quyen = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaNV = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NhanVienMaNV = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TaiKhoan_NhanVien_NhanVienMaNV",
                        column: x => x.NhanVienMaNV,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    TenSanPham = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TonDu = table.Column<float>(type: "real", nullable: false),
                    TenDonVi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MucBaoNhap = table.Column<float>(type: "real", nullable: false),
                    TenNhomSanPham = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DonViTenDonVi = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    NhomSanPhamTenNhom = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.TenSanPham);
                    table.ForeignKey(
                        name: "FK_SanPham_DonVi_DonViTenDonVi",
                        column: x => x.DonViTenDonVi,
                        principalTable: "DonVi",
                        principalColumn: "TenDonVi");
                    table.ForeignKey(
                        name: "FK_SanPham_NhomSanPham_NhomSanPhamTenNhom",
                        column: x => x.NhomSanPhamTenNhom,
                        principalTable: "NhomSanPham",
                        principalColumn: "TenNhom");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDon",
                columns: table => new
                {
                    SoHoaDon = table.Column<int>(type: "int", nullable: false),
                    MaMon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDon", x => new { x.SoHoaDon, x.MaMon });
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_HoaDon_SoHoaDon",
                        column: x => x.SoHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "SoHoaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_Mon_MaMon",
                        column: x => x.MaMon,
                        principalTable: "Mon",
                        principalColumn: "MaMon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietMon",
                columns: table => new
                {
                    TenNguyenLieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaMon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DinhLuong = table.Column<float>(type: "real", nullable: false),
                    SanPhamTenSanPham = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietMon", x => new { x.TenNguyenLieu, x.MaMon });
                    table.ForeignKey(
                        name: "FK_ChiTietMon_Mon_MaMon",
                        column: x => x.MaMon,
                        principalTable: "Mon",
                        principalColumn: "MaMon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietMon_SanPham_SanPhamTenSanPham",
                        column: x => x.SanPhamTenSanPham,
                        principalTable: "SanPham",
                        principalColumn: "TenSanPham");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCaLam_MaNV",
                table: "ChiTietCaLam",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_MaMon",
                table: "ChiTietHoaDon",
                column: "MaMon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietMon_MaMon",
                table: "ChiTietMon",
                column: "MaMon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietMon_SanPhamTenSanPham",
                table: "ChiTietMon",
                column: "SanPhamTenSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_KhuyenMaiMaKhuyenMai",
                table: "HoaDon",
                column: "KhuyenMaiMaKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_NhanVienMaNV",
                table: "HoaDon",
                column: "NhanVienMaNV");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_DonViTenDonVi",
                table: "SanPham",
                column: "DonViTenDonVi");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_NhomSanPhamTenNhom",
                table: "SanPham",
                column: "NhomSanPhamTenNhom");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_NhanVienMaNV",
                table: "TaiKhoan",
                column: "NhanVienMaNV");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ban");

            migrationBuilder.DropTable(
                name: "ChiTietCaLam");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDon");

            migrationBuilder.DropTable(
                name: "ChiTietMon");

            migrationBuilder.DropTable(
                name: "ChiTietNhap");

            migrationBuilder.DropTable(
                name: "ChucVu");

            migrationBuilder.DropTable(
                name: "LoaiCaLam");

            migrationBuilder.DropTable(
                name: "NhaCungCap");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "Mon");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "DonVi");

            migrationBuilder.DropTable(
                name: "NhomSanPham");
        }
    }
}
