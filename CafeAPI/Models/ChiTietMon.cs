using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class ChiTietMon
    {
        [Key, MaxLength(100)]
        public string? TenNguyenLieu { get; set; }
        [Key, MaxLength(20)]
        public string? MaMon { get; set; } 
        public float DinhLuong { get; set; }
        public SanPham? SanPham { get; set; } 
        public Mon? Mon { get; set; }
    }
}
