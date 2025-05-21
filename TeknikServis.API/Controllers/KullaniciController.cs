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

        [HttpPost("login")]
        public async Task<ActionResult<Kullanici>> Login( LoginModel loginModel)
        {

            if (string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Sifre))
            {
                return BadRequest("Email ve şifre boş bırakılamaz.");
            }
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(x => x.Email == loginModel.Email && x.Sifre == loginModel.Sifre);
            if (kullanici == null)
            {
                return Unauthorized("Email veya şifre hatalı.");
            }
            // ✅ JSON formatında cevap dönülüyor
            return Ok(
            kullanici);
        }

    }
}
