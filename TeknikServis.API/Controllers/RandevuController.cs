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

        [HttpGet("tarihlerandevu")]
        public async Task<ActionResult<RandevuDto>> GetRandevuByDate(DateTime tarih)
        {
            var randevular = await _context.Randevular.Where(r => r.Tarihi == tarih).Include(r => r.ServisTalebi).ThenInclude(s => s.Urun).Include(r => r.ServisTalebi.Kullanici).ToListAsync();
            if(randevular!= null && randevular.Count == 1)
            {
                Randevu r = randevular[0];
                RandevuDto dto = new RandevuDto
                {
                    ServisTalebiId = r.ServisTalebiId,
                    ServisTalebi = new ServisTalebiDto
                    {
                        Id = r.ServisTalebiId,
                        Kullanici = new KullaniciDto
                        {
                            AdSoyad = r.ServisTalebi.Kullanici.AdSoyad
                        },
                        Urun = new UrunDto
                        {
                            Ad = r.ServisTalebi.Urun.Ad
                        }
                    }

                };
                return dto;
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Randevu>>> GetRandevular()
        {
            return await _context.Randevular.Include(r => r.ServisTalebi).ThenInclude(s=>s.Urun).Include(r=>r.ServisTalebi.Kullanici).ToListAsync();
        }

        [HttpGet("randevularByKullaniciId")]
        public async Task<ActionResult<IEnumerable<Randevu>>> GetRandevularByKullaniciId(int id)
        {
            return await _context.Randevular.Include(r => r.ServisTalebi).ThenInclude(s => s.Urun).Include(r => r.ServisTalebi.Kullanici).Where(r => r.ServisTalebi.KullaniciId == id).ToListAsync();
        }


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
