using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class KhuyenMai
    {
        [Key, MaxLength(20)]
        public string? MaKhuyenMai { get; set; }
        [Required, MaxLength(100)]
        public string? TenKhuyenMai { get; set; }
        public decimal GiamGia { get; set; }
        public decimal MucApDung { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        [MaxLength(1000)]
        public string? MoTa { get; set; }
        [MaxLength(20)]
        public string? TrangThai { get; set; }
        public bool Xoa { get; set; }
    }
}
