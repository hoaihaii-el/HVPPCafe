using CafeAPI.Models;
using CafeAPI.Repo;
using CafeAPI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly DataContext _context;

        public HoaDonController(DataContext context)
        {
            _context = context;
        }

        // GET: api/HoaDon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetHoaDon()
        {
            if (_context.HoaDon == null)
            {
                return NotFound();
            }

            return await _context.HoaDon.Where(h => h.DaThanhToan).ToListAsync();
        }

        // GET: api/HoaDon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HoaDon>> GetHoaDon(int id)
        {
            if (_context.HoaDon == null)
            {
                return NotFound();
            }
            var hoaDon = await _context.HoaDon.FindAsync(id);

            if (hoaDon == null)
            {
                return NotFound();
            }

            return hoaDon;
        }

        [HttpPost]
        public async Task<ActionResult<HoaDon>> PostHoaDon(HoaDonVM hdVM)
        {
            var hoadon = new HoaDon
            {
                LoaiHoaDon = hdVM.LoaiHoaDon,
                TriGia = hdVM.TriGia,
                NgayHoaDon = hdVM.NgayHoaDon,
                MaNV = hdVM.MaNV,
                MaKhuyenMai = hdVM.MaKhuyenMai,
                DaCheBien = hdVM.DaCheBien,
                DaThanhToan = hdVM.DaThanhToan,
                SoBan = hdVM.SoBan
            };

            _context.HoaDon.Add(hoadon);
            await _context.SaveChangesAsync();

            return Ok(hoadon.SoHoaDon);
        }

        [HttpPost("add-cthd")]
        public async Task<ActionResult<ChiTietHoaDon>> AddCTHD(CTHDVM cthdVM)
        {
            var cthd = new ChiTietHoaDon
            {
                MaMon = cthdVM.MaMon,
                SoHoaDon = cthdVM.SoHoaDon,
                Size = cthdVM.Size,
                SoLuong = cthdVM.SoLuong
            };

            _context.ChiTietHoaDon.Add(cthd);
            await _context.SaveChangesAsync();

            return Ok(cthd.ID);
        }

        [HttpPost("add-cttopping")]
        public async Task<ActionResult<ChiTietTopping>> AddCTTopping(ChiTietTopping ct)
        {
            _context.ChiTietTopping.Add(ct);
            await _context.SaveChangesAsync();

            return Ok(ct);
        }
    }
}
