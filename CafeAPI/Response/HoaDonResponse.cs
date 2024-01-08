namespace CafeAPI.Response
{
    public class HoaDonResponse
    {
        public int SoHoaDon { get; set; }
        public string? LoaiHoaDon { get; set; }
        public decimal TriGia { get; set; }
        public DateTime NgayHoaDon { get; set; }
        public bool DaCheBien { get; set; }
        public bool DaThanhToan { get; set; }
        public string? HinhThucThanhToan { get; set; }
        public string? MaNV { get; set; }
        public string? TenKhuyenMai { get; set; }
        public string? GhiChu { get; set; }
        public int SoBan { get; set; }
    }
}
