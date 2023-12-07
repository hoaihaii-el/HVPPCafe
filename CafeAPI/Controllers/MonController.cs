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
    public class MonController : ControllerBase
    {
        private readonly DataContext _context;

        public MonController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Mon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mon>>> GetMon()
        {
          if (_context.Mon == null)
          {
              return NotFound();
          }
            return await _context.Mon.ToListAsync();
        }

        // GET: api/Mon/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mon>> GetMon(string id)
        {
          if (_context.Mon == null)
          {
              return NotFound();
          }
            var mon = await _context.Mon.FindAsync(id);

            if (mon == null)
            {
                return NotFound();
            }

            return mon;
        }

        // PUT: api/Mon/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMon(string id, Mon mon)
        {
            if (id != mon.MaMon)
            {
                return BadRequest();
            }

            _context.Entry(mon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonExists(id))
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

        // POST: api/Mon
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mon>> PostMon(Mon mon)
        {
          if (_context.Mon == null)
          {
              return Problem("Entity set 'DataContext.Mon'  is null.");
          }
            _context.Mon.Add(mon);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MonExists(mon.MaMon))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMon", new { id = mon.MaMon }, mon);
        }

        // DELETE: api/Mon/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMon(string id)
        {
            if (_context.Mon == null)
            {
                return NotFound();
            }
            var mon = await _context.Mon.FindAsync(id);
            if (mon == null)
            {
                return NotFound();
            }

            _context.Mon.Remove(mon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MonExists(string id)
        {
            return (_context.Mon?.Any(e => e.MaMon == id)).GetValueOrDefault();
        }
    }
}
