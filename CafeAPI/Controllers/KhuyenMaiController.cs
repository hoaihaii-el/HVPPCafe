using CafeAPI.Models;
using CafeAPI.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CafeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly DataContext _context;

        public KhuyenMaiController(DataContext context)
        {
            _context = context;
        }

        // GET: api/KhuyenMai
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhuyenMai>>> GetKhuyenMai()
        {
            var result = await _context.KhuyenMai.Where(km => !km.Xoa).ToListAsync();
            foreach (var item in result)
            {
                if (item.NgayKetThuc <= DateTime.Now)
                {
                    item.TrangThai = "Đã kết thúc";
                }
                _context.KhuyenMai.Update(item);
            }
            await _context.SaveChangesAsync();
            return result;
        }

        // GET: api/KhuyenMai/5
        [HttpGet("ap-dung/{total}")]
        public async Task<ActionResult<KeyValuePair<string, decimal>>> GetGiamGia(decimal total)
        {
            var khuyenMai = await _context.KhuyenMai
                .Where(km => km.NgayBatDau <= DateTime.Now
                          && km.NgayKetThuc >= DateTime.Now
                          && km.TrangThai == "Đang diễn ra").FirstOrDefaultAsync();

            if (khuyenMai == null) return new KeyValuePair<string, decimal>("", 0);
            if (total < khuyenMai.MucApDung) return new KeyValuePair<string, decimal>("", 0);
            return new KeyValuePair<string, decimal>(khuyenMai.MaKhuyenMai, total * khuyenMai.GiamGia / 100);
        }


        [HttpPut]
        public async Task<IActionResult> PutKhuyenMai(KhuyenMai khuyenMai)
        {
            _context.Entry(khuyenMai).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }


        [HttpPost]
        public async Task<ActionResult<KhuyenMai>> PostKhuyenMai(KhuyenMai khuyenMai)
        {
            khuyenMai.MaKhuyenMai = await AutoID();

            _context.KhuyenMai.Add(khuyenMai);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/KhuyenMai/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhuyenMai(string id)
        {
            var khuyenMai = await _context.KhuyenMai.FindAsync(id);

            if (khuyenMai == null)
            {
                return NotFound();
            }

            khuyenMai.Xoa = true;
            _context.KhuyenMai.Update(khuyenMai);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<string> AutoID()
        {
            var ID = "KM0001";

            var maxID = await _context.KhuyenMai
                .OrderByDescending(m => m.MaKhuyenMai)
                .Select(m => m.MaKhuyenMai)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maxID))
            {
                return ID;
            }

            ID = "KM";

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
