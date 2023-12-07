using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class NhomSanPham
    {
        [Key, Required, MaxLength(20)]
        public string? TenNhom { get; set; }
    }
}
