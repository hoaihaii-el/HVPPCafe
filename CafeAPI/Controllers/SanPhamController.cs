using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeAPI.Models;
using CafeAPI.Repo;
using CafeAPI.Response;
using System.Drawing;
using Humanizer.Localisation;

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
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<SanPham>>> GetSanPham()
        {
            if (_context.SanPham == null)
            {
                return NotFound();
            }
            return await _context.SanPham.Where(p => !p.Xoa).ToListAsync();
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

        [HttpGet("chi-tiet-nhap")]
        public async Task<ActionResult<IEnumerable<CTNResponse>>> GetCTN([FromQuery] DateTime begin, [FromQuery] DateTime end)
        {
            var ctn = await _context.ChiTietNhap.Where(c => c.NgayNhap >= begin && c.NgayNhap <= end).ToListAsync();

            var list = new List<CTNResponse>();
            foreach (var item in ctn)
            {
                var sp = await _context.SanPham.FindAsync(item.TenSanPham);

                list.Add(new CTNResponse
                {
                    MaNhap = item.MaNhap,
                    TenSanPham = item.TenSanPham,
                    SoLuong = item.SoLuong,
                    DonVi = item.DonVi,
                    GiaNhap = item.GiaNhap,
                    NgayNhap = item.NgayNhap,
                    NguonNhap = item.NhaCungCap,
                    LienLac = item.LienLac,
                    Nhom = sp.Nhom,
                    MucBaoNhap = sp.MucBaoNhap
                });
            }

            return list;
        }


        [HttpGet("ctn/{TenSP}")]
        public async Task<ActionResult<IEnumerable<CTNResponse>>> GetCTN2(string TenSP)
        {
            var ctn = await _context.ChiTietNhap.Where(c => c.TenSanPham == TenSP).ToListAsync();

            var list = new List<CTNResponse>();
            foreach (var item in ctn)
            {
                var sp = await _context.SanPham.FindAsync(item.TenSanPham);

                list.Add(new CTNResponse
                {
                    MaNhap = item.MaNhap,
                    TenSanPham = item.TenSanPham,
                    SoLuong = item.SoLuong,
                    DonVi = item.DonVi,
                    GiaNhap = item.GiaNhap,
                    NgayNhap = item.NgayNhap,
                    NguonNhap = item.NhaCungCap,
                    LienLac = item.LienLac,
                    Nhom = sp.Nhom,
                    MucBaoNhap = sp.MucBaoNhap
                });
            }

            return list;
        }

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

            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<SanPham>> PostSanPham(CTNResponse sanPham)
        {
            var sp = await _context.SanPham.FindAsync(sanPham.TenSanPham);

            if (sp == null)
            {
                sp = new SanPham()
                {
                    TenSanPham = sanPham.TenSanPham,
                    DonVi = sanPham.DonVi,
                    Nhom = sanPham.Nhom,
                    MucBaoNhap = sanPham.MucBaoNhap,
                    TonDu = sanPham.SoLuong
                };
                _context.SanPham.Add(sp);
            }
            else
            {
                sp.TonDu += sanPham.SoLuong;
                if (sp.Xoa)
                {
                    sp.Xoa = false;
                    sp.TonDu = sanPham.SoLuong;
                }
                _context.SanPham.Update(sp);
            }

            _context.ChiTietNhap.Add(new ChiTietNhap
            {
                TenSanPham = sp.TenSanPham,
                DonVi = sp.DonVi,
                SoLuong = sanPham.SoLuong,
                GiaNhap = sanPham.GiaNhap,
                NgayNhap = sanPham.NgayNhap,
                NhaCungCap = sanPham.NguonNhap,
                LienLac = sanPham.LienLac
            });

            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/SanPham/5
        [HttpDelete("{TenSP}")]
        public async Task<IActionResult> DeleteSanPham(string TenSP)
        {
            var sanPham = await _context.SanPham.FindAsync(TenSP);
            if (sanPham == null)
            {
                return NotFound();
            }

            sanPham.Xoa = true;
            _context.SanPham.Update(sanPham);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
