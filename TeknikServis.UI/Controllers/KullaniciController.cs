using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TeknikServis.UI.Models.dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TeknikServis.UI.Controllers
{

    [AllowAnonymous] // ⬅ Giriş kontrol filtresinden muaf
    public class KullaniciController : Controller
    {

        private readonly HttpClient _httpClient;

        public KullaniciController()
        {
            _httpClient = new HttpClient();
        }

        public IActionResult Kayit()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string sifre, bool beniHatirla)
        {

            LoginModel loginModel = new LoginModel
            {
                Email = email,
                Sifre = sifre
            };
            var jsonData = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44365/api/Kullanici/Login", content);

            Kullanici kullanici = response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<Kullanici>(await response.Content.ReadAsStringAsync())
                : null;

            if (!response.IsSuccessStatusCode)
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
