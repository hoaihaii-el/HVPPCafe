using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class HoaDon
    {
        [Key]
        public int SoHoaDon { get; set; }
        [Required, MaxLength(20)]
        public string? LoaiHoaDon { get; set; }
        public decimal TriGia { get; set; }
        public DateTime NgayHoaDon { get; set; }
        public bool DaCheBien { get; set; }
        public bool DaThanhToan { get; set; }
        [MaxLength(20)]
        public string? MaNV { get; set; }
        [MaxLength(20)]
        public string? MaKhuyenMai { get; set; }
        public int SoBan { get; set; }
        public KhuyenMai? KhuyenMai { get; set; }
        public NhanVien? NhanVien { get; set; }
    }
}
