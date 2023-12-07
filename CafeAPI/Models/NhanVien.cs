using System;
using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class NhanVien
    {
        [Key, MaxLength(20)]
        public string? MaNV { get; set; }
        [Required, MaxLength(50)]
        public string? HoTen { get; set; }
        [MaxLength(100)]
        public string? DiaChi { get; set; }
        [MaxLength(20)]
        public string? SoDienThoai { get; set; }
        public DateTime NgaySinh { get; set; }
        [MaxLength(20)]
        public string? ChucVu { get; set; }
        public DateTime NgayVaoLam { get; set; }
        public bool Xoa { get; set; }
    }
}
