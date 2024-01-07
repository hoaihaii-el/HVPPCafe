using CafeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeAPI.Repo
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Ban> Ban { get; set; }
        public DbSet<ChiTietCaLam> ChiTietCaLam { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDon { get; set; }
        public DbSet<ChiTietMon> ChiTietMon { get; set; }
        public DbSet<ChiTietNhap> ChiTietNhap { get; set; }
        public DbSet<ChucVu> ChucVu { get; set; }
        public DbSet<DonVi> DonVi { get; set; }
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<KhuyenMai> KhuyenMai { get; set; }
        public DbSet<LoaiCaLam> LoaiCaLam { get; set; }
        public DbSet<Mon> Mon { get; set; }
        public DbSet<NhaCungCap> NhaCungCap { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<NhomSanPham> NhomSanPham { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<ChiTietGia> ChiTietGia { get; set; }
        public DbSet<Topping> Topping { get; set; }
        public DbSet<ChiTietTopping> ChiTietTopping { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietCaLam>()
                .HasKey(c => new { c.LoaiCaLam, c.MaNV, c.Ngay });

            modelBuilder.Entity<ChiTietMon>()
                .HasKey(c => new { c.TenNguyenLieu, c.MaMon });

            modelBuilder.Entity<ChiTietGia>()
                .HasKey(c => new { c.MaMon, c.Size });

            modelBuilder.Entity<ChiTietTopping>()
                .HasKey(c => new { c.ID, c.TenTopping });

            base.OnModelCreating(modelBuilder);
        }
    }
}
