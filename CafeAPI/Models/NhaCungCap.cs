using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeAPI.Models
{
    public class NhaCungCap
    {
        [Key, Required, MaxLength(100)]
        public string? TenNhaCungCap { get; set; }
        [MaxLength(20)]
        public string? LienLac {  get; set; }
    }
}
