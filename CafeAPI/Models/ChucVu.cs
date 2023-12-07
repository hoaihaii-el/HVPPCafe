using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class ChucVu
    {
        [Key, MaxLength(20)]
        public string? TenChucVu { get; set; }
        public bool Part_time { get; set; }
        public decimal MucLuong { get; set; }
    }
}
