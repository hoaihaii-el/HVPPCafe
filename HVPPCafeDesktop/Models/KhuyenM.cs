using System;

namespace HVPPCafeDesktop.Models
{
    public class KhuyenM
    {
        public string? MaKhuyenMai { get; set; } = "";
        public string? TenKhuyenMai { get; set; }
        public decimal GiamGia { get; set; }
        public decimal MucApDung { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string MoTa { get; set; } = "";
        public string TrangThai { get; set; } = "";
        public bool Xoa { get; set; } = false;
    }
}
