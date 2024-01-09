using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVPPCafeDesktop.Models
{
    class CongThuc
    {
        public string TenSanPham { get; set; } = "";
        public float TonDu { get; set; }
        public string DonVi { get; set; } = "";
        public float MucBaoNhap { get; set; }
        public string Nhom { get; set; } = "";
        public bool DuocChon { get; set; }
        public float DinhLuong { get; set; }
    }
}
