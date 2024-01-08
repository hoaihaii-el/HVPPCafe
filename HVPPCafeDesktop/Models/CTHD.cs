using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVPPCafeDesktop.Models
{
    public class CTHD
    {
        public int SoHoaDon { get; set; }
        public string? MaMon { get; set; }
        public string? TenMon { get; set; }
        public string? Size { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get; set; }
        public DateTime NgayHoaDon { get; set; }
        public int SoBan { get; set; }
        public List<string> Toppings { get; set; } = new List<string>();
    }
}
