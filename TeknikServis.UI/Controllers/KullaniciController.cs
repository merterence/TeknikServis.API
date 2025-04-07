using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TeknikServis.API.Models;
using Microsoft.EntityFrameworkCore;

namespace TeknikServis.UI.Controllers
{
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
        public IActionResult Login(string email, string sifre)
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

            // Session’a kullanıcıyı ekle
            HttpContext.Session.SetString("adSoyad", kullanici.AdSoyad ?? "");
            HttpContext.Session.SetString("email", kullanici.Email ?? "");

            return RedirectToAction("Index", "Home");
        }

        // ✅ Çıkış metodu eklendi
        public IActionResult Cikis()
        {
            HttpContext.Session.Clear(); // Tüm oturum bilgilerini siler
            return RedirectToAction("Index", "Home"); // Anasayfaya yönlendirir
        }
    }
}
