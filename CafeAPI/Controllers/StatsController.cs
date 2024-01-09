using CafeAPI.Repo;
using CafeAPI.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly DataContext _context;

        public StatsController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("so-luong/{month}/{year}")]
        public async Task<ActionResult<IEnumerable<StatCrowd>>> GetStatsCrowd(int month, int year)
        {
            var result = await _context.HoaDon
                .Where(h => h.DaThanhToan && h.NgayHoaDon.Month == month && h.NgayHoaDon.Year == year)
                .GroupBy(h => h.NgayHoaDon.Day)
                .Select(group => new StatCrowd
                {
                    Day = group.Key,
                    Count = group.Count()
                })
                .ToListAsync();

            return result;
        }

        [HttpGet("ban-chay/{month}/{year}")]
        public async Task<ActionResult<IEnumerable<Stat2>>> GetStats2(int month, int year)
        {
            var result = await _context.HoaDon
                .Join(_context.ChiTietHoaDon,
                      hd => hd.SoHoaDon,
                      ct => ct.SoHoaDon,
                      (hd, ct) => new { HoaDon = hd, ChiTiet = ct })
                .Where(joined => joined.HoaDon.DaThanhToan == true
                                 && joined.HoaDon.NgayHoaDon.Month == month
                                 && joined.HoaDon.NgayHoaDon.Year == year)
                .GroupBy(joined => joined.ChiTiet.MaMon)
                .Select(group => new Stat2
                {
                    TenMon = group.Key,
                    SoLuong = group.Sum(item => item.ChiTiet.SoLuong)
                })
                .ToListAsync();

            foreach (var item in result)
            {
                item.TenMon = await _context.Mon.Where(m => m.MaMon == item.TenMon).Select(m => m.TenMon).FirstOrDefaultAsync();
            }

            return result;
        }

        [HttpGet("thu/{type}/{month}/{year}")]
        public async Task<ActionResult<IEnumerable<Profit>>> GetProfit(string type, int month, int year, [FromQuery] string begin, [FromQuery] string end)
        {
            if (type == "Ngày")
            {
                var result = await _context.HoaDon
                    .Where(hd => hd.DaThanhToan 
                        && hd.NgayHoaDon.Date >= DateTime.Parse(begin).Date
                        && hd.NgayHoaDon.Date <= DateTime.Parse(end).Date)
                    .GroupBy(hd => hd.NgayHoaDon.Day)
                    .Select(group => new Profit
                    {
                        Time = group.Key,
                        Value = group.Sum(hd => hd.TriGia)
                    })
                    .ToListAsync();

                return result;
            }
            else if (type == "Tháng")
            {
                var result = await _context.HoaDon
                    .Where(hd => hd.DaThanhToan && hd.NgayHoaDon.Month == month && hd.NgayHoaDon.Year == year)
                    .GroupBy(hd => hd.NgayHoaDon.Day)
                    .Select(group => new Profit
                    {
                        Time = group.Key,
                        Value = group.Sum(hd => hd.TriGia)
                    })
                    .ToListAsync();

                return result;
            }
            else
            {
                var result = await _context.HoaDon
                    .Where(hd => hd.DaThanhToan && hd.NgayHoaDon.Year == year)
                    .GroupBy(hd => hd.NgayHoaDon.Month)
                    .Select(group => new Profit
                    {
                        Time = group.Key,
                        Value = group.Sum(hd => hd.TriGia)
                    })
                    .ToListAsync();

                return result;
            }
        }

        [HttpGet("chi/{type}/{month}/{year}")]
        public async Task<ActionResult<IEnumerable<Profit>>> GetPaid(string type, int month, int year,[FromQuery] string begin, [FromQuery] string end)
        {
            if (type == "Ngày")
            {
                var result = await _context.ChiTietNhap
                    .Where(ct => ct.NgayNhap.Date >= DateTime.Parse(begin).Date
                              && ct.NgayNhap.Date <= DateTime.Parse(end).Date)
                    .GroupBy(hd => hd.NgayNhap.Day)
                    .Select(group => new Profit
                    {
                        Time = group.Key,
                        Value = group.Sum(hd => (decimal)hd.SoLuong * hd.GiaNhap)
                    })
                    .ToListAsync();

                return result;
            }
            else if (type == "Tháng")
            {
                var result = await _context.ChiTietNhap
                    .Where(hd => hd.NgayNhap.Month == month && hd.NgayNhap.Year == year)
                    .GroupBy(hd => hd.NgayNhap.Day)
                    .Select(group => new Profit
                    {
                        Time = group.Key,
                        Value = group.Sum(hd => (decimal)hd.SoLuong * hd.GiaNhap)
                    })
                    .ToListAsync();

                return result;
            }
            else
            {
                var result = await _context.ChiTietNhap
                    .Where(hd => hd.NgayNhap.Year == year)
                    .GroupBy(hd => hd.NgayNhap.Month)
                    .Select(group => new Profit
                    {
                        Time = group.Key,
                        Value = group.Sum(hd => (decimal)hd.SoLuong * hd.GiaNhap)
                    })
                    .ToListAsync();

                return result;
            }
        }
    }
}
