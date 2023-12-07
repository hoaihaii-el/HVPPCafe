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
    public class NhomSanPhamController : ControllerBase
    {
        private readonly DataContext _context;

        public NhomSanPhamController(DataContext context)
        {
            _context = context;
        }

        // GET: api/NhomSanPham
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhomSanPham>>> GetNhomSanPham()
        {
          if (_context.NhomSanPham == null)
          {
              return NotFound();
          }
            return await _context.NhomSanPham.ToListAsync();
        }

        // GET: api/NhomSanPham/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhomSanPham>> GetNhomSanPham(string id)
        {
          if (_context.NhomSanPham == null)
          {
              return NotFound();
          }
            var nhomSanPham = await _context.NhomSanPham.FindAsync(id);

            if (nhomSanPham == null)
            {
                return NotFound();
            }

            return nhomSanPham;
        }

        // PUT: api/NhomSanPham/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhomSanPham(string id, NhomSanPham nhomSanPham)
        {
            if (id != nhomSanPham.TenNhom)
            {
                return BadRequest();
            }

            _context.Entry(nhomSanPham).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhomSanPhamExists(id))
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

        // POST: api/NhomSanPham
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NhomSanPham>> PostNhomSanPham(NhomSanPham nhomSanPham)
        {
          if (_context.NhomSanPham == null)
          {
              return Problem("Entity set 'DataContext.NhomSanPham'  is null.");
          }
            _context.NhomSanPham.Add(nhomSanPham);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NhomSanPhamExists(nhomSanPham.TenNhom))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNhomSanPham", new { id = nhomSanPham.TenNhom }, nhomSanPham);
        }

        // DELETE: api/NhomSanPham/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhomSanPham(string id)
        {
            if (_context.NhomSanPham == null)
            {
                return NotFound();
            }
            var nhomSanPham = await _context.NhomSanPham.FindAsync(id);
            if (nhomSanPham == null)
            {
                return NotFound();
            }

            _context.NhomSanPham.Remove(nhomSanPham);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NhomSanPhamExists(string id)
        {
            return (_context.NhomSanPham?.Any(e => e.TenNhom == id)).GetValueOrDefault();
        }
    }
}
