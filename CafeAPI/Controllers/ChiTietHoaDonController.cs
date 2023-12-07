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
    public class ChiTietHoaDonController : ControllerBase
    {
        private readonly DataContext _context;

        public ChiTietHoaDonController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ChiTietHoaDon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiTietHoaDon>>> GetChiTietHoaDon()
        {
          if (_context.ChiTietHoaDon == null)
          {
              return NotFound();
          }
            return await _context.ChiTietHoaDon.ToListAsync();
        }

        // GET: api/ChiTietHoaDon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChiTietHoaDon>> GetChiTietHoaDon(int id)
        {
          if (_context.ChiTietHoaDon == null)
          {
              return NotFound();
          }
            var chiTietHoaDon = await _context.ChiTietHoaDon.FindAsync(id);

            if (chiTietHoaDon == null)
            {
                return NotFound();
            }

            return chiTietHoaDon;
        }

        // PUT: api/ChiTietHoaDon/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChiTietHoaDon(int id, ChiTietHoaDon chiTietHoaDon)
        {
            if (id != chiTietHoaDon.SoHoaDon)
            {
                return BadRequest();
            }

            _context.Entry(chiTietHoaDon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiTietHoaDonExists(id))
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

        // POST: api/ChiTietHoaDon
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChiTietHoaDon>> PostChiTietHoaDon(ChiTietHoaDon chiTietHoaDon)
        {
          if (_context.ChiTietHoaDon == null)
          {
              return Problem("Entity set 'DataContext.ChiTietHoaDon'  is null.");
          }
            _context.ChiTietHoaDon.Add(chiTietHoaDon);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChiTietHoaDonExists(chiTietHoaDon.SoHoaDon))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChiTietHoaDon", new { id = chiTietHoaDon.SoHoaDon }, chiTietHoaDon);
        }

        // DELETE: api/ChiTietHoaDon/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChiTietHoaDon(int id)
        {
            if (_context.ChiTietHoaDon == null)
            {
                return NotFound();
            }
            var chiTietHoaDon = await _context.ChiTietHoaDon.FindAsync(id);
            if (chiTietHoaDon == null)
            {
                return NotFound();
            }

            _context.ChiTietHoaDon.Remove(chiTietHoaDon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChiTietHoaDonExists(int id)
        {
            return (_context.ChiTietHoaDon?.Any(e => e.SoHoaDon == id)).GetValueOrDefault();
        }
    }
}
