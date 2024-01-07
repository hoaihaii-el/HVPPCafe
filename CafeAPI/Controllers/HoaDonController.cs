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

            return await _context.HoaDon.Where(h => h.TrangThai == "Đã thanh toán").ToListAsync();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoaDon(int id, HoaDon hoaDon)
        {
            if (id != hoaDon.SoHoaDon)
            {
                return BadRequest();
            }

            _context.Entry(hoaDon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<HoaDon>> PostHoaDon(HoaDon hoaDon)
        {
            if (_context.HoaDon == null)
            {
                return Problem("Entity set 'DataContext.HoaDon'  is null.");
            }

            _context.HoaDon.Add(hoaDon);
            await _context.SaveChangesAsync();

            return Ok(hoaDon);
        }

        // DELETE: api/HoaDon/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoaDon(int id)
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

            _context.HoaDon.Remove(hoaDon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HoaDonExists(int id)
        {
            return (_context.HoaDon?.Any(e => e.SoHoaDon == id)).GetValueOrDefault();
        }
    }
}
