namespace CafeAPI.Response
{
    public class CTHDResponse
    {
        public int ID { get; set; }
        public int SoHoaDon { get; set; }
        public string? MaMon { get; set; }
        public string? TenMon { get; set; }
        public string? Size { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }
        public string? LoaiHoaDon { get; set; }
        public DateTime NgayHoaDon { get; set; }
        public int SoBan { get; set; }  
        public List<string> Toppings { get; set; } = new List<string>();
    }
}
