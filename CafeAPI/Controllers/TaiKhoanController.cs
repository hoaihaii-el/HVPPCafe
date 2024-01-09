using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeAPI.Models;
using CafeAPI.Repo;
using CafeAPI.StaticServices;

namespace CafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly DataContext _context;

        public TaiKhoanController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("{TK}/{MK}")]
        public async Task<ActionResult<TaiKhoan>> DangNhap(string TK, string MK)
        {
            var taikhoan = await _context.TaiKhoan.Where(t => t.ID == TK && t.MatKhau == MK).FirstOrDefaultAsync();

            if (taikhoan == null) 
            {
                return NotFound();
            }

            return taikhoan;
        }

        // GET: api/TaiKhoan/5
        [HttpGet("{MaNV}")]
        public async Task<ActionResult<TaiKhoan>> GetTaiKhoan(string MaNV)
        {
            var taiKhoan = await _context.TaiKhoan.FindAsync(MaNV);

            if (taiKhoan == null)
            {
                return NotFound();
            }

            return taiKhoan;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaiKhoan(string id, TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.ID)
            {
                return BadRequest();
            }

            _context.Entry(taiKhoan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaiKhoanExists(id))
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


        [HttpGet("tham-so")]
        public async Task<ActionResult<Dictionary<string, string>>> GetThamSo()
        {
            var dic = new Dictionary<string, string>();

            var thamso = await _context.ThamSo.ToListAsync();
            foreach (var item in thamso)
            {
                dic.Add(item.Ten, item.GiaTri);
            }

            return dic;
        }

        [HttpPut("set-tham-so")]
        public async Task<ActionResult> SetThamSo(ThamSo thamso)
        {
            var ts = await _context.ThamSo.FindAsync(thamso.Ten);
            if (ts != null)
            {
                _context.ThamSo.Remove(ts);
            }

            if (thamso.Ten == "QrThanhToan")
            {
                thamso.GiaTri = await UploadImage.Instance.UploadAsync(Guid.NewGuid().ToString(), thamso.GiaTri);
            }
            _context.ThamSo.Add(thamso);

            return Ok();
        }


        [HttpPost]
        public async Task<ActionResult<TaiKhoan>> PostTaiKhoan(TaiKhoan taiKhoan)
        {
          if (_context.TaiKhoan == null)
          {
              return Problem("Entity set 'DataContext.TaiKhoan'  is null.");
          }
            _context.TaiKhoan.Add(taiKhoan);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TaiKhoanExists(taiKhoan.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTaiKhoan", new { id = taiKhoan.ID }, taiKhoan);
        }

        // DELETE: api/TaiKhoan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaiKhoan(string id)
        {
            if (_context.TaiKhoan == null)
            {
                return NotFound();
            }
            var taiKhoan = await _context.TaiKhoan.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            _context.TaiKhoan.Remove(taiKhoan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaiKhoanExists(string id)
        {
            return (_context.TaiKhoan?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
