using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class ThamSo
    {
        [Key]
        public string Ten { get; set; } = "";
        public string GiaTri { get; set; } = "";
    }
}
