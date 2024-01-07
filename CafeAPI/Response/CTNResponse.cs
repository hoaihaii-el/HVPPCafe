namespace CafeAPI.Response
{
    public class CTNResponse
    {
        public int MaNhap { get; set; }
        public string TenSanPham { get; set; } = "";
        public float SoLuong { get; set; }
        public string DonVi { get; set; } = "";
        public float MucBaoNhap { get; set; }
        public decimal GiaNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public string Nhom { get; set; } = "";
        public string NguonNhap { get; set; } = "";
        public string LienLac { get; set; } = "";
    }
}
