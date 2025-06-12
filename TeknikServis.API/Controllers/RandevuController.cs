using Microsoft.AspNetCore.Mvc;
using TeknikServis.API.Models;
using Microsoft.EntityFrameworkCore;
using TeknikServis.DTO;

namespace TeknikServis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandevuController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RandevuController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public async Task<ActionResult<Randevu>> GetRandevuById(int id)
        //{
        //    return await _context.Randevular.Include(r=>r.ServisTalebi).ThenInclude(s=>s.Kullanici).FirstOrDefaultAsync(r=>r.Id == id);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Randevu>>> GetRandevular()
        {
            return await _context.Randevular.Include(r => r.ServisTalebi).ThenInclude(s => s.Kullanici).ToListAsync();
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Randevu>>> GetRandevularByKullaniciId(int kullaniciId)
        //{
        //    return await _context.Randevular.Where(r=> r.ServisTalebi.KullaniciId == kullaniciId).ToListAsync();
        //}


        [HttpPost]
        public async Task<ActionResult<Randevu>> PostRandevu([FromBody]RandevuDto randevuDto)
        {
            Randevu randevu = new Randevu
            {
                ServisTalebiId = randevuDto.ServisTalebiId,
                Tarihi = randevuDto.Tarihi,
                RandevuDurumu = randevuDto.RandevuDurumu.Value
            };
            _context.Randevular.Add(randevu);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRandevular", new { id = randevu.Id }, randevu);
        }

        //[HttpPut]
        //public async Task<IActionResult> PutRandevu([FromBody] RandevuDto dto)
        //{

        //    var mevcut = _context.Randevular.FirstOrDefault(x => x.Id == dto.Id);
        //    if (mevcut == null)
        //    {
        //        return NotFound();
        //    }


        //    mevcut.RandevuDurumu = dto.RandevuDurumu.Value;


        //    try
        //    {
        //        _context.Randevular.Update(mevcut);
        //        _context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {

        //    }


        //    // ✅ JSON formatında cevap dönülüyor
        //    return Ok();
        //}


    }
}
