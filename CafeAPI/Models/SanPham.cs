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
        public string? TenDonVi { get; set; }
        public float MucBaoNhap { get; set; }
        [Required, MaxLength(20)]
        public string? TenNhomSanPham { get; set; }
        public DonVi? DonVi { get; set; }
        public NhomSanPham? NhomSanPham { get; set; }
    }
}
