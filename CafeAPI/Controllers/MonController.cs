using CafeAPI.Models;
using CafeAPI.Repo;
using CafeAPI.Response;
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
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<MonResponse>>> GetAll()
        {
            if (_context.Mon == null)
            {
                return NotFound();
            }

            var menu = await _context.Mon.Where(m => !m.Xoa).ToListAsync();

            var result = new List<MonResponse>();

            foreach (var item in menu)
            {
                var response = new MonResponse
                {
                    MaMon = item.MaMon,
                    TenMon = item.TenMon,
                    AnhMonAn = item.AnhMonAn,
                    Nhom = item.Nhom,
                    GiaBanL = 0,
                    GiaBanXL = 0,
                    TyLeL = 0,
                    TyLeXL = 0
                };
                var ctg = await _context.ChiTietGia.Where(c => c.MaMon == item.MaMon).ToListAsync();
                if (ctg == null) continue;

                foreach (var ct in ctg)
                {
                    if (ct.Size == "M")
                    {
                        response.GiaBanM = ct.GiaBan;
                    }
                    if (ct.Size == "L")
                    {
                        response.GiaBanL = ct.GiaBan;
                        response.TyLeL = ct.TyLeSizeM;
                    }
                    if (ct.Size == "XL")
                    {
                        response.GiaBanXL = ct.GiaBan;
                        response.TyLeXL = ct.TyLeSizeM;
                    }
                }

                result.Add(response);
            }

            return result;
        }


        [HttpGet("toppings")]
        public async Task<ActionResult<IEnumerable<Topping>>> GetTopping()
        {
            return await _context.Topping.ToListAsync();
        }


        [HttpPost("add-topping")]
        public async Task<ActionResult<Topping>> AddTopping(ToppingVM toppingVM)
        {
            var topping = new Topping
            {
                TenTopping = toppingVM.TenTopping,
                Gia = toppingVM.Gia,
            };

            _context.Topping.Add(topping);
            await _context.SaveChangesAsync();

            return Ok(topping);
        }


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
            mon.Nhom = monVM.Nhom;
            
            if (!string.IsNullOrEmpty(monVM.AnhMonAn))
            {
                mon.AnhMonAn = await UploadImage.Instance.UploadAsync(maMon, monVM.AnhMonAn);
            }

            if (monVM.GiaBanM > 0)
            {
                var ctg = await _context.ChiTietGia.FindAsync(maMon, "M");
                if (ctg == null) return NotFound();
                ctg.GiaBan = monVM.GiaBanM;

                _context.ChiTietGia.Update(ctg);
            }
            if (monVM.GiaBanL > 0)
            {
                var ctg = await _context.ChiTietGia.FindAsync(maMon, "L");
                if (ctg == null) return NotFound();
                ctg.GiaBan = monVM.GiaBanL;
                ctg.TyLeSizeM = monVM.TyLeL;

                _context.ChiTietGia.Update(ctg);
            }
            if (monVM.GiaBanXL > 0)
            {
                var ctg = await _context.ChiTietGia.FindAsync(maMon, "XL");
                if (ctg == null) return NotFound();
                ctg.GiaBan = monVM.GiaBanXL;
                ctg.TyLeSizeM = monVM.TyLeXL;

                _context.ChiTietGia.Update(ctg);
            }

            _context.Mon.Update(mon);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Mon
        [HttpPost]
        public async Task<ActionResult<Mon>> PostMon(MonVM monVM)
        {
            var mon = new Mon
            {
                MaMon = await AutoID(),
                TenMon = monVM.TenMon,
                Nhom = monVM.Nhom,
                Xoa = false
            };

            mon.AnhMonAn = await UploadImage.Instance.UploadAsync(mon.MaMon, monVM.AnhMonAn ?? "");

            _context.Mon.Add(mon);

            if (monVM.GiaBanM > 0)
            {
                _context.ChiTietGia.Add(new ChiTietGia
                {
                    MaMon = mon.MaMon,
                    Size = "M",
                    GiaBan = monVM.GiaBanM,
                    TyLeSizeM = 1
                });
            }

            if (monVM.GiaBanL > 0)
            {
                _context.ChiTietGia.Add(new ChiTietGia
                {
                    MaMon = mon.MaMon,
                    Size = "L",
                    GiaBan = monVM.GiaBanL,
                    TyLeSizeM = monVM.TyLeL
                });
            }

            if (monVM.GiaBanXL > 0)
            {
                _context.ChiTietGia.Add(new ChiTietGia
                {
                    MaMon = mon.MaMon,
                    Size = "XL",
                    GiaBan = monVM.GiaBanXL,
                    TyLeSizeM = monVM.TyLeXL
                });
            }

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
        [HttpDelete("{maMon}")]
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
