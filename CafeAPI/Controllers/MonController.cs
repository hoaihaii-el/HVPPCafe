using CafeAPI.Models;
using CafeAPI.Repo;
using CafeAPI.StaticServices;
using CafeAPI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

            return await _context.Mon.Where(m => !m.Xoa).ToListAsync();
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
        [HttpPut("{maMon}")]
        public async Task<IActionResult> PutMon(string maMon, MonVM monVM)
        {
            var mon = await _context.Mon.FindAsync(maMon);            

            if (mon == null)
            {
                return NotFound("Món không tồn tại!");
            }

            mon.TenMon = monVM.TenMon;
            mon.GiaBan = monVM.GiaBan;
            mon.Nhom = monVM.Nhom;
            mon.AnhMonAn = await UploadImage.Instance.UploadAsync(maMon, monVM.AnhMonAn ?? "");

            try
            {
                _context.Mon.Update(mon);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Lưu thất bại!");
            }

            return NoContent();
        }

        // POST: api/Mon
        [HttpPost]
        public async Task<ActionResult<Mon>> PostMon(MonVM monVM)
        {
            var mon = new Mon
            {
                MaMon = await AutoID(),
                TenMon = monVM.TenMon,
                GiaBan = monVM.GiaBan,
                Nhom = monVM.Nhom,
                Xoa = false
            };

            mon.AnhMonAn = await UploadImage.Instance.UploadAsync(mon.MaMon, monVM.AnhMonAn ?? "");

            _context.Mon.Add(mon);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new
                {
                    message = "Món đã tồn tại!"
                });
            }

            return Ok(mon);
        }

        // DELETE: api/Mon/5
        [HttpPut("xoa/{maMon}")]
        public async Task<IActionResult> DeleteMon(string maMon)
        {
            var mon = await _context.Mon.FindAsync(maMon);

            if (mon == null)
            {
                return NotFound();
            }

            mon.Xoa = true;

            _context.Mon.Update(mon);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true
            });
        }

        private async Task<string> AutoID()
        {
            var ID = "MA0001";

            var maxID = await _context.Mon
                .OrderByDescending(m => m.MaMon)
                .Select(m => m.MaMon)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maxID))
            {
                return ID;
            }

            ID = "MA";

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
