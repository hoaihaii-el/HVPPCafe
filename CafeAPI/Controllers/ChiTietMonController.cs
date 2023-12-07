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
    public class ChiTietMonController : ControllerBase
    {
        private readonly DataContext _context;

        public ChiTietMonController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ChiTietMon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiTietMon>>> GetChiTietMon()
        {
          if (_context.ChiTietMon == null)
          {
              return NotFound();
          }
            return await _context.ChiTietMon.ToListAsync();
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

        // PUT: api/ChiTietMon/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChiTietMon(string id, ChiTietMon chiTietMon)
        {
            if (id != chiTietMon.TenNguyenLieu)
            {
                return BadRequest();
            }

            _context.Entry(chiTietMon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiTietMonExists(id))
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

        // POST: api/ChiTietMon
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChiTietMon>> PostChiTietMon(ChiTietMon chiTietMon)
        {
          if (_context.ChiTietMon == null)
          {
              return Problem("Entity set 'DataContext.ChiTietMon'  is null.");
          }
            _context.ChiTietMon.Add(chiTietMon);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChiTietMonExists(chiTietMon.TenNguyenLieu))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChiTietMon", new { id = chiTietMon.TenNguyenLieu }, chiTietMon);
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
