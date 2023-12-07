using CafeAPI.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace CafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietCaLamController : ControllerBase
    {
        private readonly DataContext _context;

        public ChiTietCaLamController(DataContext context)
        {
            _context = context;
        }
    }
}
