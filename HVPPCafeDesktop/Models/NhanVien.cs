using System;

namespace HVPPCafeDesktop.Models
{
    class NhanVien
    {
        public string? MaNV { get; set; }
        public string? HoTen { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public bool IsFullTime { get; set; }
        public DateTime NgaySinh { get; set; }
        public string? ChucVu { get; set; }
        public DateTime NgayVaoLam { get; set; }
        public bool Xoa { get; set; }
    }
}
