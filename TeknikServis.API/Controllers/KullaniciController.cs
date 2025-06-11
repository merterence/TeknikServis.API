using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Drawing;
using TeknikServis.API.Models;
using TeknikServis.API.Models.Dto;
using TeknikServis.DTO;

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


        [HttpPut]
        public async Task<IActionResult> Guncelle([FromBody] KullaniciDto dto)
        {

            var mevcut = _context.Kullanicilar.Include(k => k.Adres).FirstOrDefault(x => x.Id == dto.Id);
            if (mevcut == null)
            {
                return NotFound();
            }


            mevcut.AdSoyad = dto.AdSoyad;
            if (mevcut.Adres != null)
            {
                mevcut.Adres.Sehir = dto.AdresDto.Sehir;
                mevcut.Adres.Ilce = dto.AdresDto.Ilce;
                mevcut.Adres.Mahalle = dto.AdresDto.Mahalle;
                mevcut.Adres.Sokak = dto.AdresDto.Sokak;
                mevcut.Adres.No = dto.AdresDto.No;
            }
            else
            {
            
            mevcut.Adres = new Adres
            {
                Sehir = dto.AdresDto.Sehir,
                Ilce = dto.AdresDto.Ilce,
                Mahalle = dto.AdresDto.Mahalle,
                Sokak = dto.AdresDto.Sokak,
                No = dto.AdresDto.No
            };
            }
            

            try
            {
                _context.Kullanicilar.Update(mevcut);
                _context.SaveChanges();
            }catch(Exception ex)
            {

            }
          

            // ✅ JSON formatında cevap dönülüyor
            return Ok();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<KullaniciDto>> GetKullaniciById([FromRoute]int id)
        {
            var kullanici = await _context.Kullanicilar.Include(k=>k.Adres)
               .FirstOrDefaultAsync(x => x.Id == id);
            if (kullanici == null)
            {
                return NotFound();
            }
            // ✅ JSON formatında cevap dönülüyor


            AdresDto adresDto = null;
            if(kullanici.Adres != null)
            {
                adresDto = new AdresDto
                {
                    Sehir = kullanici.Adres.Sehir,
                    Ilce = kullanici.Adres.Ilce,
                    Mahalle = kullanici.Adres.Mahalle,
                    Sokak = kullanici.Adres.Sokak,
                    No = kullanici.Adres.No
                };
            }
          

            KullaniciDto kullaniciDto = new KullaniciDto
            {
                Id = kullanici.Id,
                AdSoyad = kullanici.AdSoyad,
                AdresDto = adresDto,
                Email = kullanici.Email
            };
            return Ok(kullaniciDto);
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
        public async Task<ActionResult<KullaniciDto>> Login([FromForm]string email,[FromForm] string sifre)
        {
   
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
            {
                return BadRequest("Email ve şifre boş bırakılamaz.");
            }
            var kullaniciDto = await _context.Kullanicilar
                .FirstOrDefaultAsync(x => x.Email == email && x.Sifre == sifre);
            if (kullaniciDto == null)
            {
                return Unauthorized("Email veya şifre hatalı.");
            }
            // ✅ JSON formatında cevap dönülüyor
            return Ok(kullaniciDto);
        }

    }
}
