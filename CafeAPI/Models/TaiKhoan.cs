using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class TaiKhoan
    {
        [Key, MaxLength(20)]
        public string? ID { get; set; }
        [MaxLength(20)]
        public string? MatKhau { get; set; }
        [MaxLength(20)]
        public string? Quyen { get; set; }
        [MaxLength(20)]
        public string? MaNV { get; set; }
        public NhanVien? NhanVien { get; set; }
    }
}
