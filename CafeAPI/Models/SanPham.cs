using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class SanPham
    {
        [Key, Required, MaxLength(100)]
        public string? TenSanPham { get; set; }
        [Required]
        public float TonDu { get; set; }
        [Required, MaxLength(10)]
        public string? DonVi { get; set; }
        public float MucBaoNhap { get; set; }
        [Required, MaxLength(20)]
        public string? Nhom { get; set; }
        public bool Xoa { get; set; }
    }
}
