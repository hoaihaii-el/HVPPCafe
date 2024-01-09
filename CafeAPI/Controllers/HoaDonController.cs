using CafeAPI.Models;
using CafeAPI.Repo;
using CafeAPI.Response;
using CafeAPI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly DataContext _context;

        public HoaDonController(DataContext context)
        {
            _context = context;
        }

        // GET: api/HoaDon
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<HoaDonResponse>>> GetHoaDon()
        {
            var bills = await _context.HoaDon.Where(h => h.DaThanhToan).ToListAsync();

            var result = new List<HoaDonResponse>();
            foreach (var bill in bills)
            {
                var km = await _context.KhuyenMai.FindAsync(bill.MaKhuyenMai);

                result.Add(new HoaDonResponse
                {
                    SoHoaDon = bill.SoHoaDon,
                    LoaiHoaDon = bill.LoaiHoaDon,
                    TriGia = bill.TriGia,
                    NgayHoaDon = bill.NgayHoaDon,
                    HinhThucThanhToan = bill.HinhThucThanhToan,
                    SoBan = bill.SoBan,
                    MaNV = bill.MaNV,
                    TenKhuyenMai = km != null ? km.TenKhuyenMai : ""
                });
            }

            return result;
        }

        [HttpGet("cthd-all/{SoHD}")]
        public async Task<ActionResult<IEnumerable<CTHDResponse>>> GetCTHD(int SoHD)
        {
            var bill = await _context.HoaDon.FindAsync(SoHD);
            if (bill == null) return NotFound();
            var result = new List<CTHDResponse>();

            var cthd = await _context.ChiTietHoaDon.Where(o => o.SoHoaDon == SoHD).ToListAsync();
            foreach (var ct in cthd)
            {
                var mon = await _context.Mon.FindAsync(ct.MaMon);
                var ctg = await _context.ChiTietGia.FindAsync(ct.MaMon, ct.Size);
                result.Add(new CTHDResponse
                {
                    SoHoaDon = ct.SoHoaDon,
                    MaMon = ct.MaMon,
                    TenMon = mon.TenMon,
                    Size =ct.Size,
                    SoLuong = ct.SoLuong,
                    ThanhTien = ct.SoLuong * ctg.GiaBan
                });
                var cttoppings = await _context.ChiTietTopping.Where(t => t.ID == ct.ID).ToListAsync(); 
                foreach (var ctt in cttoppings)
                {
                    result[result.Count - 1].Toppings.Add(ctt.TenTopping);
                }
            }
            return result;
        }

        [HttpGet("chebien")]
        public async Task<ActionResult<IEnumerable<CTHDResponse>>> GetCheBien()
        {
            var bills = await _context.HoaDon
                .Where(b => !b.DaCheBien)
                .OrderBy(b => b.NgayHoaDon)
                .ToListAsync();

            if (bills == null) return NotFound();

            var result = new List<CTHDResponse>();

            foreach (var bill in bills)
            {
                var cthd = await _context.ChiTietHoaDon.Where(o => o.SoHoaDon == bill.SoHoaDon).ToListAsync();
                foreach (var ct in cthd)
                {
                    var mon = await _context.Mon.FindAsync(ct.MaMon);
                    result.Add(new CTHDResponse
                    {
                        ID = ct.ID,
                        SoHoaDon = ct.SoHoaDon,
                        MaMon = ct.MaMon,
                        TenMon = mon.TenMon,
                        Size = ct.Size,
                        SoLuong = ct.SoLuong,
                        LoaiHoaDon = bill.LoaiHoaDon,
                        NgayHoaDon = bill.NgayHoaDon
                    });
                    var cttoppings = await _context.ChiTietTopping.Where(t => t.ID == ct.ID).ToListAsync();
                    if (cttoppings == null) continue;
                    foreach (var ctt in cttoppings)
                    {
                        result[result.Count - 1].Toppings.Add(ctt.TenTopping);
                    }
                }
            }
            return result;
        }

        // GET: api/HoaDon/5
        [HttpDelete("chebien/{SoHD}")]
        public async Task<ActionResult<HoaDon>> CheBienXong(int SoHD)
        {
            var hoadon = await _context.HoaDon.FindAsync(SoHD);

            if (hoadon == null) return NotFound();
            hoadon.DaCheBien = true;
            _context.HoaDon.Update(hoadon);

            var cthd = await _context.ChiTietHoaDon.Where(ct => ct.SoHoaDon == SoHD).ToListAsync();
            foreach(var ct in cthd)
            {
                var tyle = await _context.ChiTietGia
                    .Where(c => c.MaMon == ct.MaMon && c.Size == ct.Size)
                    .Select(c => c.TyLeSizeM)
                    .FirstOrDefaultAsync();

                var nl = await _context.ChiTietMon.Where(m => m.MaMon == ct.MaMon).ToListAsync();
                foreach (var nguyenlieu in nl)
                {
                    var sp = await _context.SanPham.FindAsync(nguyenlieu.TenNguyenLieu);
                    sp.TonDu -= (float)(nguyenlieu.DinhLuong * tyle);
                }
            }

            await _context.SaveChangesAsync();

            return hoadon;
        }

        [HttpPost]
        public async Task<ActionResult<HoaDon>> PostHoaDon(HoaDonVM hdVM)
        {
            var hoadon = new HoaDon
            {
                LoaiHoaDon = hdVM.LoaiHoaDon,
                TriGia = hdVM.TriGia,
                NgayHoaDon = hdVM.NgayHoaDon,
                MaNV = hdVM.MaNV,
                MaKhuyenMai = hdVM.MaKhuyenMai,
                DaCheBien = hdVM.DaCheBien,
                DaThanhToan = hdVM.DaThanhToan,
                SoBan = hdVM.SoBan,
                HinhThucThanhToan = hdVM.HinhThucThanhToan,
                GhiChu = hdVM.GhiChu
            };

            _context.HoaDon.Add(hoadon);
            await _context.SaveChangesAsync();

            return Ok(hoadon.SoHoaDon);
        }

        [HttpPost("add-cthd")]
        public async Task<ActionResult<ChiTietHoaDon>> AddCTHD(CTHDVM cthdVM)
        {
            var cthd = new ChiTietHoaDon
            {
                MaMon = cthdVM.MaMon,
                SoHoaDon = cthdVM.SoHoaDon,
                Size = cthdVM.Size,
                SoLuong = cthdVM.SoLuong
            };

            _context.ChiTietHoaDon.Add(cthd);
            await _context.SaveChangesAsync();

            return Ok(cthd.ID);
        }

        [HttpPost("add-cttopping")]
        public async Task<ActionResult<ChiTietTopping>> AddCTTopping(ChiTietTopping ct)
        {
            _context.ChiTietTopping.Add(ct);
            await _context.SaveChangesAsync();

            return Ok(ct);
        }
    }
}
