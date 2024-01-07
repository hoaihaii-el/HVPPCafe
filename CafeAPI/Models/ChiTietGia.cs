using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class ChiTietGia
    {
        [Key, MaxLength(20)]
        public string? MaMon { get; set; }
        [Key, MaxLength(10)]
        public string? Size { get; set; }
        public decimal GiaBan { get; set; }
        //tỷ lệ nguyên liệu cần thiết so với sizeM
        public double TyLeSizeM { get; set; }
    }
}
