using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class ChiTietCaLam
    {
        [Key, Required, MaxLength(20)]
        public string? MaNV { get; set; }
        [Key]
        public DateTime Ngay { get; set; }
        public double SoGio { get; set; }
        public string? GhiChu { get; set; }
        [JsonIgnore]
        public NhanVien? NhanVien { get; set; }
    }
}
