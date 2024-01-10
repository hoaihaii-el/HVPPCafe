using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVPPCafe.Models
{
    public class Menu
    {
        public string? MaMon { get; set; }
        public string? TenMon { get; set; }
        public string? AnhMonAn { get; set; }
        public string? Nhom { get; set; }
        public decimal GiaBanM { get; set; }
        public decimal GiaBanL { get; set; }
        public decimal GiaBanXL { get; set; }
        public double TyLeL { get; set; }
        public double TyLeXL { get; set; }
    }
}
