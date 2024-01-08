namespace CafeAPI.Response
{
    public class NhanVienResponse
    {
        public string? MaNV { get; set; }
        public string? HoTen { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public bool IsFullTime { get; set; }
        public DateTime NgaySinh { get; set; }
        public string? ChucVu { get; set; }
        public DateTime NgayVaoLam { get; set; }
        public string? TaiKhoan { get; set; }
        public string? MatKhau { get; set; }
    }
}
