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
    public class KhuyenMaiController : ControllerBase
    {
        private readonly DataContext _context;

        public KhuyenMaiController(DataContext context)
        {
            _context = context;
        }

        // GET: api/KhuyenMai
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhuyenMai>>> GetKhuyenMai()
        {
          if (_context.KhuyenMai == null)
          {
              return NotFound();
          }
            return await _context.KhuyenMai.ToListAsync();
        }

        // GET: api/KhuyenMai/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KhuyenMai>> GetKhuyenMai(string id)
        {
          if (_context.KhuyenMai == null)
          {
              return NotFound();
          }
            var khuyenMai = await _context.KhuyenMai.FindAsync(id);

            if (khuyenMai == null)
            {
                return NotFound();
            }

            return khuyenMai;
        }

        // PUT: api/KhuyenMai/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKhuyenMai(string id, KhuyenMai khuyenMai)
        {
            if (id != khuyenMai.MaKhuyenMai)
            {
                return BadRequest();
            }

            _context.Entry(khuyenMai).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhuyenMaiExists(id))
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

        // POST: api/KhuyenMai
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KhuyenMai>> PostKhuyenMai(KhuyenMai khuyenMai)
        {
          if (_context.KhuyenMai == null)
          {
              return Problem("Entity set 'DataContext.KhuyenMai'  is null.");
          }
            _context.KhuyenMai.Add(khuyenMai);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KhuyenMaiExists(khuyenMai.MaKhuyenMai))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetKhuyenMai", new { id = khuyenMai.MaKhuyenMai }, khuyenMai);
        }

        // DELETE: api/KhuyenMai/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhuyenMai(string id)
        {
            if (_context.KhuyenMai == null)
            {
                return NotFound();
            }
            var khuyenMai = await _context.KhuyenMai.FindAsync(id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            _context.KhuyenMai.Remove(khuyenMai);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KhuyenMaiExists(string id)
        {
            return (_context.KhuyenMai?.Any(e => e.MaKhuyenMai == id)).GetValueOrDefault();
        }
    }
}
