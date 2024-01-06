using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class Topping
    {
        [Key]
        public int ID { get; set; }
        public string? TenTopping { get; set; }
        public decimal? Gia { get; set; }
    }
}
