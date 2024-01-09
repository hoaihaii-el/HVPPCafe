using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeAPI.Models;
using CafeAPI.Repo;
using CafeAPI.Response;

namespace CafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietMonController : ControllerBase
    {
        private readonly DataContext _context;

        public ChiTietMonController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ChiTietMon
        [HttpGet("cong-thuc/{MaMon}")]
        public async Task<ActionResult<IEnumerable<CongThucResponse>>> GetNguyenLieu(string MaMon)
        {
            var result = await _context.SanPham.Where(p => !p.Xoa)
                .Select(p => new CongThucResponse
                {
                    TenSanPham = p.TenSanPham,
                    DonVi = p.DonVi,
                    Nhom = p.Nhom,
                    MucBaoNhap = p.MucBaoNhap,
                    TonDu = p.TonDu,
                    DuocChon = false,
                    DinhLuong = 0
                })
                .ToListAsync();

            var ct = await _context.ChiTietMon.Where(c => c.MaMon == MaMon).ToListAsync();
            foreach (var c in ct)
            {
                foreach (var item in result)
                {
                    if (item.TenSanPham == c.TenNguyenLieu)
                    {
                        item.DuocChon = true;
                        item.DinhLuong = c.DinhLuong;
                    }
                }
            }
            return result;
        }

        // GET: api/ChiTietMon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChiTietMon>> GetChiTietMon(string id)
        {
          if (_context.ChiTietMon == null)
          {
              return NotFound();
          }
            var chiTietMon = await _context.ChiTietMon.FindAsync(id);

            if (chiTietMon == null)
            {
                return NotFound();
            }

            return chiTietMon;
        }

        [HttpPut]
        public async Task<IActionResult> PutChiTietMon(ChiTietMon chiTietMon)
        {
            var ct = await _context.ChiTietMon.FindAsync(chiTietMon.TenNguyenLieu, chiTietMon.MaMon);

            if (ct != null)
            {
                _context.ChiTietMon.Remove(ct);
            }

            _context.ChiTietMon.Add(chiTietMon);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<ChiTietMon>> PostChiTietMon(ChiTietMon chiTietMon)
        {
            _context.ChiTietMon.Add(chiTietMon);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/ChiTietMon/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChiTietMon(string id)
        {
            if (_context.ChiTietMon == null)
            {
                return NotFound();
            }
            var chiTietMon = await _context.ChiTietMon.FindAsync(id);
            if (chiTietMon == null)
            {
                return NotFound();
            }

            _context.ChiTietMon.Remove(chiTietMon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChiTietMonExists(string id)
        {
            return (_context.ChiTietMon?.Any(e => e.TenNguyenLieu == id)).GetValueOrDefault();
        }
    }
}
