using CafeAPI.Models;
using CafeAPI.Repo;
using CafeAPI.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("cham-cong/{month}/{year}")]
        public async Task<ActionResult<IEnumerable<ChamCong>>> GetChamCong(int month, int year)
        {
            var result = await _context.NhanVien.Where(n => !n.Xoa)
                .Select(n => new ChamCong
                {
                    MaNV = n.MaNV,
                    HoTen = n.HoTen,
                    ChucVu = n.ChucVu,
                    IsFullTime = n.IsFullTime,
                    TongSogio = 0
                })
                .ToListAsync();

            foreach (var cc in result)
            {
                var tong = await _context.ChiTietCaLam
                    .Where(n => n.MaNV == cc.MaNV && n.Ngay.Month == month && n.Ngay.Year == year)
                    .OrderBy(n => n.MaNV)
                    .Select(n => n.SoGio)
                    .SumAsync();
                cc.TongSogio = tong;
            }
            return result;
        }

        [HttpGet("chi-tiet-cc")]
        public async Task<ActionResult<IEnumerable<ChiTietCC>>> GetChiTietCC([FromQuery] string date)
        {
            var ct = await _context.NhanVien.Where(n => !n.Xoa)
                .Select(n => new ChiTietCC
                {
                    MaNV = n.MaNV,
                    Ngay = DateTime.Parse(date),
                    SoGio = 0,
                    GhiChu = ""
                })
                .ToListAsync();

            foreach (var c in ct)
            {
                var item = await _context.ChiTietCaLam.Where(cc => cc.MaNV == c.MaNV && cc.Ngay == c.Ngay).FirstOrDefaultAsync();
                if (item != null)
                {
                    c.SoGio = item.SoGio;
                    c.GhiChu = item.GhiChu;
                }
            }

            return ct;
        }

        [HttpPut("chi-tiet")]
        public async Task<ActionResult> SetChiTietCC(ChiTietCC cc)
        {
            var ct = await _context.ChiTietCaLam
                .Where(c => c.Ngay.Date == cc.Ngay.Date && c.MaNV == cc.MaNV)
                .FirstOrDefaultAsync();

            if (ct != null)
            {
                _context.ChiTietCaLam.Remove(ct);
            }
            _context.ChiTietCaLam.Add(new ChiTietCaLam
            {
                MaNV = cc.MaNV,
                Ngay = cc.Ngay,
                SoGio = cc.SoGio,
                GhiChu = cc.GhiChu
            });
            await _context.SaveChangesAsync();

            return Ok();
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
