using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TeknikServis.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TeknikServis.UI.Controllers
{
    [AllowAnonymous] // ⬅ Giriş kontrol filtresinden muaf
    public class KullaniciController : Controller
    {
        public IActionResult Kayit()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string sifre, bool beniHatirla)
        {
            // DbContext bağlantısı
            var context = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TeknikServisDB;Trusted_Connection=True;")
                .Options);

            var kullanici = context.Kullanicilar
                .FirstOrDefault(x => x.Email == email && x.Sifre == sifre);

            if (kullanici == null)
            {
                ViewBag.Mesaj = "Email veya şifre hatalı.";
                return View();
            }

            // Session’a kullanıcı bilgilerini kaydet
            HttpContext.Session.SetString("adSoyad", kullanici.AdSoyad ?? "");
            HttpContext.Session.SetString("email", kullanici.Email ?? "");
            HttpContext.Session.SetInt32("kullaniciId", kullanici.Id); // ➡️ Kullanıcı ID bilgisini de Session'a kaydediyoruz

            // ✅ 4. Başarılı giriş olursa LocalStorage'a ID aktaracağız (Login.cshtml dosyasında)
            TempData["kullaniciId"] = kullanici.Id;

            // ✅ Eğer beni hatırla seçiliyse cookie ekle
            if (beniHatirla)
            {
                Response.Cookies.Append("email", email, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7)
                });

                Response.Cookies.Append("sifre", sifre, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7)
                });
            }
            else
            {
                Response.Cookies.Delete("email");
                Response.Cookies.Delete("sifre");
            }

            return RedirectToAction("Index", "Home");
        }

        // ✅ Çıkış metodu
        public IActionResult Cikis()
        {
            HttpContext.Session.Clear(); // Tüm oturum bilgilerini temizle
            return RedirectToAction("Index", "Home");
        }
    }
}
