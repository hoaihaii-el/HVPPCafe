using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class Mon
    {
        [Key, MaxLength(20)]
        public string? MaMon { get; set; }
        [Required, MaxLength(100)]
        public string? TenMon { get; set; }
        [MaxLength(1000)]
        public string? AnhMonAn { get; set; }
        public decimal GiaBan { get; set; }
        public string? Nhom { get; set; }
        public bool Xoa { get; set; }
    }
}
