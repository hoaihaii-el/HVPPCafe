using System;
using System.ComponentModel.DataAnnotations;
using CafeAPI.Repo;

namespace CafeAPI.Models
{
    public class ChiTietCaLam
    {
        [Key]
        public int LoaiCaLam { get; set; }
        [Key, Required, MaxLength(20)]
        public string? MaNV { get; set; }
        [Key]
        public DateTime Ngay { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string? GhiChu { get; set; }
        public NhanVien? NhanVien { get; set; }
        public LoaiCaLam? LoaiCa { get; set; }
    }
}
