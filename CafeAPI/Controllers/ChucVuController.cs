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
    public class ChucVuController : ControllerBase
    {
        private readonly DataContext _context;

        public ChucVuController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ChucVu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChucVu>>> GetChucVu()
        {
          if (_context.ChucVu == null)
          {
              return NotFound();
          }
            return await _context.ChucVu.ToListAsync();
        }

        // GET: api/ChucVu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChucVu>> GetChucVu(string id)
        {
          if (_context.ChucVu == null)
          {
              return NotFound();
          }
            var chucVu = await _context.ChucVu.FindAsync(id);

            if (chucVu == null)
            {
                return NotFound();
            }

            return chucVu;
        }

        // PUT: api/ChucVu/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChucVu(string id, ChucVu chucVu)
        {
            if (id != chucVu.TenChucVu)
            {
                return BadRequest();
            }

            _context.Entry(chucVu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChucVuExists(id))
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

        // POST: api/ChucVu
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChucVu>> PostChucVu(ChucVu chucVu)
        {
          if (_context.ChucVu == null)
          {
              return Problem("Entity set 'DataContext.ChucVu'  is null.");
          }
            _context.ChucVu.Add(chucVu);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChucVuExists(chucVu.TenChucVu))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChucVu", new { id = chucVu.TenChucVu }, chucVu);
        }

        // DELETE: api/ChucVu/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChucVu(string id)
        {
            if (_context.ChucVu == null)
            {
                return NotFound();
            }
            var chucVu = await _context.ChucVu.FindAsync(id);
            if (chucVu == null)
            {
                return NotFound();
            }

            _context.ChucVu.Remove(chucVu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChucVuExists(string id)
        {
            return (_context.ChucVu?.Any(e => e.TenChucVu == id)).GetValueOrDefault();
        }
    }
}
