using System.Collections.Generic;

namespace HVPPCafeDesktop.Models
{
    class NewOrder
    {
        public int Index { get; set; }
        public string? MaMon { get; set; }
        public string? TenMon { get; set; }
        public string? Size { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public bool CoTopping { get; set; }
        public List<Topping> toppings { get; set; } = new List<Topping>();
    }
}
