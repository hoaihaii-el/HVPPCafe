using System.ComponentModel.DataAnnotations;

namespace CafeAPI.Models
{
    public class DonVi
    {
        [Key, Required, MaxLength(10)]
        public string? TenDonVi { get; set; }
    }
}
