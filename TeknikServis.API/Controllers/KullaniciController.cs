using Microsoft.AspNetCore.Mvc;
using TeknikServis.API.Models;

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
        public IActionResult KayitOl([FromBody] Kullanici kullanici)
        {
            if (string.IsNullOrEmpty(kullanici.Email) || string.IsNullOrEmpty(kullanici.Sifre))
            {
                return BadRequest("Email ve şifre boş bırakılamaz.");
            }

            // Zaten var mı diye kontrol
            var mevcut = _context.Kullanicilar.FirstOrDefault(x => x.Email == kullanici.Email);
            if (mevcut != null)
            {
                return Conflict("Bu email ile kayıtlı kullanıcı zaten var.");
            }

            _context.Kullanicilar.Add(kullanici);
            _context.SaveChanges();

            // ✅ JSON formatında cevap dönülüyor
            return Ok(new
            {

                mesaj = "Kayıt başarılı.",
                yonlendir = "/Kullanici/Login"
            });
        }

        [HttpPost("giris")]
        public IActionResult Giris([FromBody] Kullanici kullanici)
        {
            if (string.IsNullOrEmpty(kullanici.Email) || string.IsNullOrEmpty(kullanici.Sifre))
                return BadRequest("Email ve şifre boş olamaz.");

            var mevcut = _context.Kullanicilar.FirstOrDefault(x =>
                x.Email == kullanici.Email && x.Sifre == kullanici.Sifre);

            if (mevcut == null)
                return Unauthorized("Email veya şifre hatalı.");

            return Ok("Giriş başarılı.");
        }
    }
}
