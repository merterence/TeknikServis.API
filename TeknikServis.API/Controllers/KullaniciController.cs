using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknikServis.API.Models;
using TeknikServis.API.Models.Dto;

namespace TeknikServis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KullaniciController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> KayitOl([FromBody] KullaniciKayitDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Sifre))
            {
                return BadRequest("Email ve şifre boş bırakılamaz.");
            }

            // Zaten var mı diye kontrol
            var mevcut = _context.Kullanicilar.FirstOrDefault(x => x.Email == dto.Email);
            if (mevcut != null)
            {
                return Conflict("Bu email ile kayıtlı kullanıcı zaten var.");
            }
            var kullanici = new Kullanici
            {
                AdSoyad = dto.AdSoyad,
                Email = dto.Email,
                Sifre = dto.Sifre,
                IsAdmin = false
            };

            _context.Kullanicilar.Add(kullanici);
            _context.SaveChanges();

            // ✅ JSON formatında cevap dönülüyor
            return Ok(new
            {

                mesaj = "Kayıt başarılı.",
                yonlendir = "/Kullanici/Login"
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<Kullanici>> Login([FromForm]string email,[FromForm] string sifre)
        {
   
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
            {
                return BadRequest("Email ve şifre boş bırakılamaz.");
            }
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(x => x.Email == email && x.Sifre == sifre);
            if (kullanici == null)
            {
                return Unauthorized("Email veya şifre hatalı.");
            }
            // ✅ JSON formatında cevap dönülüyor
            return Ok(kullanici);
        }

    }
}
