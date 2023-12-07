using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class LoaiCaLam
    {
        [Key]
        public int SoThuTu { get; set; }
        [MaxLength(20)]
        public string? TenCa { get; set; }
        public float GioBatDau { get; set; }
        public float GioKetThuc { get; set; }
        public float HeSoLuong { get; set; }
    }
}
