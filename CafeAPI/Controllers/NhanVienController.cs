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
using System.Text.RegularExpressions;

namespace CafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly DataContext _context;

        public NhanVienController(DataContext context)
        {
            _context = context;
        }

        // GET: api/NhanVien
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<NhanVienResponse>>> GetNhanVien()
        {
            var nv = await _context.NhanVien.Where(nv => !nv.Xoa).ToListAsync();

            var result = new List<NhanVienResponse>();
            foreach (var n in nv)
            {
                var tk = await _context.TaiKhoan.Where(t => t.MaNV == n.MaNV).FirstOrDefaultAsync();

                result.Add(new NhanVienResponse
                {
                    MaNV = n.MaNV,
                    HoTen = n.HoTen,
                    DiaChi = n.DiaChi,
                    SoDienThoai = n.SoDienThoai,
                    IsFullTime = n.IsFullTime,
                    NgaySinh = n.NgaySinh,
                    ChucVu = n.ChucVu,
                    NgayVaoLam = n.NgayVaoLam,
                    TaiKhoan = tk != null ? tk.ID : "",
                    MatKhau = tk != null ? tk.MatKhau : ""
                });
            }
            return result;
        }

        // GET: api/NhanVien/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> GetNhanVien(string id)
        {
          if (_context.NhanVien == null)
          {
              return NotFound();
          }
            var nhanVien = await _context.NhanVien.FindAsync(id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            return nhanVien;
        }

        [HttpPut]
        public async Task<IActionResult> PutNhanVien(NhanVienResponse n)
        {
            var nhanVien = await _context.NhanVien.FindAsync(n.MaNV);

            if (nhanVien == null) return NotFound();
            nhanVien.HoTen = n.HoTen;
            nhanVien.DiaChi = n.DiaChi;
            nhanVien.SoDienThoai = n.SoDienThoai;
            nhanVien.IsFullTime = n.IsFullTime;
            nhanVien.NgaySinh = n.NgaySinh;
            nhanVien.ChucVu = n.ChucVu;
            nhanVien.NgayVaoLam = n.NgayVaoLam;

            _context.NhanVien.Update(nhanVien);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostNhanVien(NhanVienResponse nvVM)
        {
            var nhanVien = new NhanVien
            {
                MaNV = await AutoID(),
                HoTen = nvVM.HoTen,
                DiaChi = nvVM.DiaChi,
                SoDienThoai = nvVM.SoDienThoai,
                IsFullTime = nvVM.IsFullTime,
                NgaySinh = nvVM.NgaySinh,
                ChucVu = nvVM.ChucVu,
                NgayVaoLam = nvVM.NgayVaoLam
            };

            _context.NhanVien.Add(nhanVien);

            if (!string.IsNullOrEmpty(nvVM.TaiKhoan))
            {
                var tk = new TaiKhoan
                {
                    ID = nvVM.TaiKhoan,
                    MatKhau = nvVM.MatKhau,
                    MaNV = nhanVien.MaNV,
                    Quyen = "Nhân viên"
                };
                _context.TaiKhoan.Add(tk);
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/NhanVien/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanVien(string id)
        {
            var nhanVien = await _context.NhanVien.FindAsync(id);
            if (nhanVien == null) return NotFound();
            nhanVien.Xoa = true;

            _context.NhanVien.Update(nhanVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string> AutoID()
        {
            var ID = "NV0001";

            var maxID = await _context.NhanVien
                .OrderByDescending(m => m.MaNV)
                .Select(m => m.MaNV)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maxID))
            {
                return ID;
            }

            ID = "NV";

            var numeric = Regex.Match(maxID, @"\d+").Value;

            numeric = (int.Parse(numeric) + 1).ToString();

            while (ID.Length + numeric.Length < 6)
            {
                ID += '0';
            }

            return ID + numeric;
        }
    }
}
