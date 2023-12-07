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
    public class LoaiCaLamController : ControllerBase
    {
        private readonly DataContext _context;

        public LoaiCaLamController(DataContext context)
        {
            _context = context;
        }

        // GET: api/LoaiCaLam
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiCaLam>>> GetLoaiCaLam()
        {
          if (_context.LoaiCaLam == null)
          {
              return NotFound();
          }
            return await _context.LoaiCaLam.ToListAsync();
        }

        // GET: api/LoaiCaLam/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoaiCaLam>> GetLoaiCaLam(int id)
        {
          if (_context.LoaiCaLam == null)
          {
              return NotFound();
          }
            var loaiCaLam = await _context.LoaiCaLam.FindAsync(id);

            if (loaiCaLam == null)
            {
                return NotFound();
            }

            return loaiCaLam;
        }

        // PUT: api/LoaiCaLam/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoaiCaLam(int id, LoaiCaLam loaiCaLam)
        {
            if (id != loaiCaLam.SoThuTu)
            {
                return BadRequest();
            }

            _context.Entry(loaiCaLam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiCaLamExists(id))
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

        // POST: api/LoaiCaLam
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoaiCaLam>> PostLoaiCaLam(LoaiCaLam loaiCaLam)
        {
          if (_context.LoaiCaLam == null)
          {
              return Problem("Entity set 'DataContext.LoaiCaLam'  is null.");
          }
            _context.LoaiCaLam.Add(loaiCaLam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoaiCaLam", new { id = loaiCaLam.SoThuTu }, loaiCaLam);
        }

        // DELETE: api/LoaiCaLam/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoaiCaLam(int id)
        {
            if (_context.LoaiCaLam == null)
            {
                return NotFound();
            }
            var loaiCaLam = await _context.LoaiCaLam.FindAsync(id);
            if (loaiCaLam == null)
            {
                return NotFound();
            }

            _context.LoaiCaLam.Remove(loaiCaLam);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoaiCaLamExists(int id)
        {
            return (_context.LoaiCaLam?.Any(e => e.SoThuTu == id)).GetValueOrDefault();
        }
    }
}
