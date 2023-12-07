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
    public class DonViController : ControllerBase
    {
        private readonly DataContext _context;

        public DonViController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DonVi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonVi>>> GetDonVi()
        {
          if (_context.DonVi == null)
          {
              return NotFound();
          }
            return await _context.DonVi.ToListAsync();
        }

        // GET: api/DonVi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonVi>> GetDonVi(string id)
        {
          if (_context.DonVi == null)
          {
              return NotFound();
          }
            var donVi = await _context.DonVi.FindAsync(id);

            if (donVi == null)
            {
                return NotFound();
            }

            return donVi;
        }

        // PUT: api/DonVi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonVi(string id, DonVi donVi)
        {
            if (id != donVi.TenDonVi)
            {
                return BadRequest();
            }

            _context.Entry(donVi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonViExists(id))
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

        // POST: api/DonVi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DonVi>> PostDonVi(DonVi donVi)
        {
          if (_context.DonVi == null)
          {
              return Problem("Entity set 'DataContext.DonVi'  is null.");
          }
            _context.DonVi.Add(donVi);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DonViExists(donVi.TenDonVi))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDonVi", new { id = donVi.TenDonVi }, donVi);
        }

        // DELETE: api/DonVi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonVi(string id)
        {
            if (_context.DonVi == null)
            {
                return NotFound();
            }
            var donVi = await _context.DonVi.FindAsync(id);
            if (donVi == null)
            {
                return NotFound();
            }

            _context.DonVi.Remove(donVi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DonViExists(string id)
        {
            return (_context.DonVi?.Any(e => e.TenDonVi == id)).GetValueOrDefault();
        }
    }
}
