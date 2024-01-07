using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class ChiTietTopping
    {
        [Key]
        public int ID { get; set; }
        [Key]
        public string TenTopping { get; set; } = "";
    }
}