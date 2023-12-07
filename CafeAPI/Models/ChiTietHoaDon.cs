using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class ChiTietHoaDon
    {
        [Key]
        public int SoHoaDon { get; set; }
        [Key, MaxLength(20)]
        public string? MaMon { get; set; }
        public int SoLuong { get; set; }
        public HoaDon? HoaDon { get; set; }
        public Mon? Mon { get; set; }
    }
}
