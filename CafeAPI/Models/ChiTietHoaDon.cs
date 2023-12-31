﻿using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class ChiTietHoaDon
    {
        [Key]
        public int ID { get; set; }
        public int SoHoaDon { get; set; }
        [MaxLength(20)]
        public string? MaMon { get; set; }
        [MaxLength(10)]
        public string? Size { get; set; }
        public int SoLuong { get; set; }
    }
}
