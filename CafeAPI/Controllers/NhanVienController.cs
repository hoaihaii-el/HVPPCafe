using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeAPI.Models;
using CafeAPI.Repo;

namespace CafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly DataContext _context;

        public NhanVienController(DataContext context)
        {
            _context = context;
        }

        // GET: api/NhanVien
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanVien()
        {
            return await _context.NhanVien.Where(nv => !nv.Xoa).ToListAsync();
        }

        // GET: api/NhanVien/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> GetNhanVien(string id)
        {
          if (_context.NhanVien == null)
          {
              return NotFound();
          }
            var nhanVien = await _context.NhanVien.FindAsync(id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            return nhanVien;
        }

        [HttpPut]
        public async Task<IActionResult> PutNhanVien(NhanVien nhanVien)
        {
            _context.NhanVien.Update(nhanVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostNhanVien(NhanVien nhanVien)
        {
            _context.NhanVien.Add(nhanVien);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/NhanVien/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanVien(string id)
        {
            if (_context.NhanVien == null)
            {
                return NotFound();
            }
            var nhanVien = await _context.NhanVien.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            _context.NhanVien.Remove(nhanVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NhanVienExists(string id)
        {
            return (_context.NhanVien?.Any(e => e.MaNV == id)).GetValueOrDefault();
        }
    }
}
