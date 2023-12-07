using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class ChiTietNhap
    {
        [Key]
        public int MaNhap { get; set; }
        [Required, MaxLength(100)]
        public string? TenSanPham { get; set; }
        public DateTime NgayNhap { get; set; }
        public float SoLuong { get; set; }
        public decimal GiaNhap { get; set; }
        [MaxLength(10)]
        public string? TenDonVi { get; set; }
        [MaxLength(100)]
        public string? TenNhaCungCap { get; set; }
    }
}
