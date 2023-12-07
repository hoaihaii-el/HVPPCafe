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
    public class SanPhamController : ControllerBase
    {
        private readonly DataContext _context;

        public SanPhamController(DataContext context)
        {
            _context = context;
        }

        // GET: api/SanPham
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SanPham>>> GetSanPham()
        {
          if (_context.SanPham == null)
          {
              return NotFound();
          }
            return await _context.SanPham.ToListAsync();
        }

        // GET: api/SanPham/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SanPham>> GetSanPham(string id)
        {
          if (_context.SanPham == null)
          {
              return NotFound();
          }
            var sanPham = await _context.SanPham.FindAsync(id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return sanPham;
        }

        // PUT: api/SanPham/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSanPham(string id, SanPham sanPham)
        {
            if (id != sanPham.TenSanPham)
            {
                return BadRequest();
            }

            _context.Entry(sanPham).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(id))
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

        // POST: api/SanPham
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SanPham>> PostSanPham(SanPham sanPham)
        {
          if (_context.SanPham == null)
          {
              return Problem("Entity set 'DataContext.SanPham'  is null.");
          }
            _context.SanPham.Add(sanPham);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SanPhamExists(sanPham.TenSanPham))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSanPham", new { id = sanPham.TenSanPham }, sanPham);
        }

        // DELETE: api/SanPham/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSanPham(string id)
        {
            if (_context.SanPham == null)
            {
                return NotFound();
            }
            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            _context.SanPham.Remove(sanPham);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SanPhamExists(string id)
        {
            return (_context.SanPham?.Any(e => e.TenSanPham == id)).GetValueOrDefault();
        }
    }
}
