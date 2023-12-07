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
    public class NhaCungCapController : ControllerBase
    {
        private readonly DataContext _context;

        public NhaCungCapController(DataContext context)
        {
            _context = context;
        }

        // GET: api/NhaCungCap
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhaCungCap>>> GetNhaCungCap()
        {
          if (_context.NhaCungCap == null)
          {
              return NotFound();
          }
            return await _context.NhaCungCap.ToListAsync();
        }

        // GET: api/NhaCungCap/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhaCungCap>> GetNhaCungCap(string id)
        {
          if (_context.NhaCungCap == null)
          {
              return NotFound();
          }
            var nhaCungCap = await _context.NhaCungCap.FindAsync(id);

            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return nhaCungCap;
        }

        // PUT: api/NhaCungCap/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhaCungCap(string id, NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.TenNhaCungCap)
            {
                return BadRequest();
            }

            _context.Entry(nhaCungCap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhaCungCapExists(id))
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

        // POST: api/NhaCungCap
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NhaCungCap>> PostNhaCungCap(NhaCungCap nhaCungCap)
        {
          if (_context.NhaCungCap == null)
          {
              return Problem("Entity set 'DataContext.NhaCungCap'  is null.");
          }
            _context.NhaCungCap.Add(nhaCungCap);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NhaCungCapExists(nhaCungCap.TenNhaCungCap))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNhaCungCap", new { id = nhaCungCap.TenNhaCungCap }, nhaCungCap);
        }

        // DELETE: api/NhaCungCap/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhaCungCap(string id)
        {
            if (_context.NhaCungCap == null)
            {
                return NotFound();
            }
            var nhaCungCap = await _context.NhaCungCap.FindAsync(id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }

            _context.NhaCungCap.Remove(nhaCungCap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NhaCungCapExists(string id)
        {
            return (_context.NhaCungCap?.Any(e => e.TenNhaCungCap == id)).GetValueOrDefault();
        }
    }
}
