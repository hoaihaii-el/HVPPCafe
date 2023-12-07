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
    public class ChiTietNhapController : ControllerBase
    {
        private readonly DataContext _context;

        public ChiTietNhapController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ChiTietNhap
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiTietNhap>>> GetChiTietNhap()
        {
          if (_context.ChiTietNhap == null)
          {
              return NotFound();
          }
            return await _context.ChiTietNhap.ToListAsync();
        }

        // GET: api/ChiTietNhap/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChiTietNhap>> GetChiTietNhap(int id)
        {
          if (_context.ChiTietNhap == null)
          {
              return NotFound();
          }
            var chiTietNhap = await _context.ChiTietNhap.FindAsync(id);

            if (chiTietNhap == null)
            {
                return NotFound();
            }

            return chiTietNhap;
        }

        // PUT: api/ChiTietNhap/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChiTietNhap(int id, ChiTietNhap chiTietNhap)
        {
            if (id != chiTietNhap.MaNhap)
            {
                return BadRequest();
            }

            _context.Entry(chiTietNhap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiTietNhapExists(id))
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

        // POST: api/ChiTietNhap
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChiTietNhap>> PostChiTietNhap(ChiTietNhap chiTietNhap)
        {
          if (_context.ChiTietNhap == null)
          {
              return Problem("Entity set 'DataContext.ChiTietNhap'  is null.");
          }
            _context.ChiTietNhap.Add(chiTietNhap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChiTietNhap", new { id = chiTietNhap.MaNhap }, chiTietNhap);
        }

        // DELETE: api/ChiTietNhap/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChiTietNhap(int id)
        {
            if (_context.ChiTietNhap == null)
            {
                return NotFound();
            }
            var chiTietNhap = await _context.ChiTietNhap.FindAsync(id);
            if (chiTietNhap == null)
            {
                return NotFound();
            }

            _context.ChiTietNhap.Remove(chiTietNhap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChiTietNhapExists(int id)
        {
            return (_context.ChiTietNhap?.Any(e => e.MaNhap == id)).GetValueOrDefault();
        }
    }
}
