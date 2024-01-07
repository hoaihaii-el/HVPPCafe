﻿// <auto-generated />
using System;
using CafeAPI.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CafeAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240106192423_AddMenuSizeAndTopping")]
    partial class AddMenuSizeAndTopping
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CafeAPI.Models.Ban", b =>
                {
                    b.Property<int>("SoBan")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SoBan"), 1L, 1);

                    b.HasKey("SoBan");

                    b.ToTable("Ban");
                });

            modelBuilder.Entity("CafeAPI.Models.ChiTietCaLam", b =>
                {
                    b.Property<int>("LoaiCaLam")
                        .HasColumnType("int");

                    b.Property<string>("MaNV")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("Ngay")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CheckOut")
                        .HasColumnType("datetime2");

                    b.Property<string>("GhiChu")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LoaiCaSoThuTu")
                        .HasColumnType("int");

                    b.HasKey("LoaiCaLam", "MaNV", "Ngay");

                    b.HasIndex("LoaiCaSoThuTu");

                    b.HasIndex("MaNV");

                    b.ToTable("ChiTietCaLam");
                });

            modelBuilder.Entity("CafeAPI.Models.ChiTietGia", b =>
                {
                    b.Property<string>("MaMon")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Size")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal>("GiaBan")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("TyLeSizeM")
                        .HasColumnType("float");

                    b.HasKey("MaMon", "Size");

                    b.ToTable("ChiTietGia");
                });

            modelBuilder.Entity("CafeAPI.Models.ChiTietHoaDon", b =>
                {
                    b.Property<int>("SoHoaDon")
                        .HasColumnType("int");

                    b.Property<string>("MaMon")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("SoHoaDon", "MaMon");

                    b.HasIndex("MaMon");

                    b.ToTable("ChiTietHoaDon");
                });

            modelBuilder.Entity("CafeAPI.Models.ChiTietMon", b =>
                {
                    b.Property<string>("TenNguyenLieu")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("MaMon")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<float>("DinhLuong")
                        .HasColumnType("real");

                    b.Property<string>("SanPhamTenSanPham")
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TenNguyenLieu", "MaMon");

                    b.HasIndex("MaMon");

                    b.HasIndex("SanPhamTenSanPham");

                    b.ToTable("ChiTietMon");
                });

            modelBuilder.Entity("CafeAPI.Models.ChiTietNhap", b =>
                {
                    b.Property<int>("MaNhap")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNhap"), 1L, 1);

                    b.Property<decimal>("GiaNhap")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("NgayNhap")
                        .HasColumnType("datetime2");

                    b.Property<float>("SoLuong")
                        .HasColumnType("real");

                    b.Property<string>("TenDonVi")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("TenNhaCungCap")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TenSanPham")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("MaNhap");

                    b.ToTable("ChiTietNhap");
                });

            modelBuilder.Entity("CafeAPI.Models.ChucVu", b =>
                {
                    b.Property<string>("TenChucVu")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("MucLuong")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Part_time")
                        .HasColumnType("bit");

                    b.HasKey("TenChucVu");

                    b.ToTable("ChucVu");
                });

            modelBuilder.Entity("CafeAPI.Models.DonVi", b =>
                {
                    b.Property<string>("TenDonVi")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("TenDonVi");

                    b.ToTable("DonVi");
                });

            modelBuilder.Entity("CafeAPI.Models.HoaDon", b =>
                {
                    b.Property<int>("SoHoaDon")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SoHoaDon"), 1L, 1);

                    b.Property<string>("KhuyenMaiMaKhuyenMai")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LoaiHoaDon")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MaKhuyenMai")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MaNV")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("NgayHoaDon")
                        .HasColumnType("datetime2");

                    b.Property<string>("NhanVienMaNV")
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("SoBan")
                        .HasColumnType("int");

                    b.Property<string>("TrangThai")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("TriGia")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("SoHoaDon");

                    b.HasIndex("KhuyenMaiMaKhuyenMai");

                    b.HasIndex("NhanVienMaNV");

                    b.ToTable("HoaDon");
                });

            modelBuilder.Entity("CafeAPI.Models.KhuyenMai", b =>
                {
                    b.Property<string>("MaKhuyenMai")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("GiamGia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("MoTa")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<decimal>("MucApDung")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("NgayBatDau")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayKetThuc")
                        .HasColumnType("datetime2");

                    b.Property<string>("TenKhuyenMai")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TrangThai")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("Xoa")
                        .HasColumnType("bit");

                    b.HasKey("MaKhuyenMai");

                    b.ToTable("KhuyenMai");
                });

            modelBuilder.Entity("CafeAPI.Models.LoaiCaLam", b =>
                {
                    b.Property<int>("SoThuTu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SoThuTu"), 1L, 1);

                    b.Property<float>("GioBatDau")
                        .HasColumnType("real");

                    b.Property<float>("GioKetThuc")
                        .HasColumnType("real");

                    b.Property<float>("HeSoLuong")
                        .HasColumnType("real");

                    b.Property<string>("TenCa")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("SoThuTu");

                    b.ToTable("LoaiCaLam");
                });

            modelBuilder.Entity("CafeAPI.Models.Mon", b =>
                {
                    b.Property<string>("MaMon")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("AnhMonAn")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<decimal>("GiaBan")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nhom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenMon")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Xoa")
                        .HasColumnType("bit");

                    b.HasKey("MaMon");

                    b.ToTable("Mon");
                });

            modelBuilder.Entity("CafeAPI.Models.NhaCungCap", b =>
                {
                    b.Property<string>("TenNhaCungCap")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LienLac")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("TenNhaCungCap");

                    b.ToTable("NhaCungCap");
                });

            modelBuilder.Entity("CafeAPI.Models.NhanVien", b =>
                {
                    b.Property<string>("MaNV")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ChucVu")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("DiaChi")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("NgaySinh")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("NgayVaoLam")
                        .HasColumnType("datetime2");

                    b.Property<string>("SoDienThoai")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("Xoa")
                        .HasColumnType("bit");

                    b.HasKey("MaNV");

                    b.ToTable("NhanVien");
                });

            modelBuilder.Entity("CafeAPI.Models.NhomSanPham", b =>
                {
                    b.Property<string>("TenNhom")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("TenNhom");

                    b.ToTable("NhomSanPham");
                });

            modelBuilder.Entity("CafeAPI.Models.SanPham", b =>
                {
                    b.Property<string>("TenSanPham")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DonViTenDonVi")
                        .HasColumnType("nvarchar(10)");

                    b.Property<float>("MucBaoNhap")
                        .HasColumnType("real");

                    b.Property<string>("NhomSanPhamTenNhom")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TenDonVi")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("TenNhomSanPham")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<float>("TonDu")
                        .HasColumnType("real");

                    b.Property<bool>("Xoa")
                        .HasColumnType("bit");

                    b.HasKey("TenSanPham");

                    b.HasIndex("DonViTenDonVi");

                    b.HasIndex("NhomSanPhamTenNhom");

                    b.ToTable("SanPham");
                });

            modelBuilder.Entity("CafeAPI.Models.TaiKhoan", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MaNV")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MatKhau")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("NhanVienMaNV")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Quyen")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("ID");

                    b.HasIndex("NhanVienMaNV");

                    b.ToTable("TaiKhoan");
                });

            modelBuilder.Entity("CafeAPI.Models.Topping", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<decimal?>("Gia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TenTopping")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Topping");
                });

            modelBuilder.Entity("CafeAPI.Models.ChiTietCaLam", b =>
                {
                    b.HasOne("CafeAPI.Models.LoaiCaLam", "LoaiCa")
                        .WithMany()
                        .HasForeignKey("LoaiCaSoThuTu");

                    b.HasOne("CafeAPI.Models.NhanVien", "NhanVien")
                        .WithMany()
                        .HasForeignKey("MaNV")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoaiCa");

                    b.Navigation("NhanVien");
                });

            modelBuilder.Entity("CafeAPI.Models.ChiTietGia", b =>
                {
                    b.HasOne("CafeAPI.Models.Mon", "Mon")
                        .WithMany()
                        .HasForeignKey("MaMon")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mon");
                });

            modelBuilder.Entity("CafeAPI.Models.ChiTietHoaDon", b =>
                {
                    b.HasOne("CafeAPI.Models.Mon", "Mon")
                        .WithMany()
                        .HasForeignKey("MaMon")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CafeAPI.Models.HoaDon", "HoaDon")
                        .WithMany()
                        .HasForeignKey("SoHoaDon")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HoaDon");

                    b.Navigation("Mon");
                });

            modelBuilder.Entity("CafeAPI.Models.ChiTietMon", b =>
                {
                    b.HasOne("CafeAPI.Models.Mon", "Mon")
                        .WithMany()
                        .HasForeignKey("MaMon")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CafeAPI.Models.SanPham", "SanPham")
                        .WithMany()
                        .HasForeignKey("SanPhamTenSanPham");

                    b.Navigation("Mon");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("CafeAPI.Models.HoaDon", b =>
                {
                    b.HasOne("CafeAPI.Models.KhuyenMai", "KhuyenMai")
                        .WithMany()
                        .HasForeignKey("KhuyenMaiMaKhuyenMai");

                    b.HasOne("CafeAPI.Models.NhanVien", "NhanVien")
                        .WithMany()
                        .HasForeignKey("NhanVienMaNV");

                    b.Navigation("KhuyenMai");

                    b.Navigation("NhanVien");
                });

            modelBuilder.Entity("CafeAPI.Models.SanPham", b =>
                {
                    b.HasOne("CafeAPI.Models.DonVi", "DonVi")
                        .WithMany()
                        .HasForeignKey("DonViTenDonVi");

                    b.HasOne("CafeAPI.Models.NhomSanPham", "NhomSanPham")
                        .WithMany()
                        .HasForeignKey("NhomSanPhamTenNhom");

                    b.Navigation("DonVi");

                    b.Navigation("NhomSanPham");
                });

            modelBuilder.Entity("CafeAPI.Models.TaiKhoan", b =>
                {
                    b.HasOne("CafeAPI.Models.NhanVien", "NhanVien")
                        .WithMany()
                        .HasForeignKey("NhanVienMaNV");

                    b.Navigation("NhanVien");
                });
#pragma warning restore 612, 618
        }
    }
}