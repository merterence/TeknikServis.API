using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TeknikServis.UI.Models.dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TeknikServis.DTO;
using System.Reflection;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guncelle([Bind("AdSoyad,AdresDto")] KullaniciDto kullaniciDto)
        {
            kullaniciDto.Id = HttpContext.Session.GetInt32("kullaniciId").Value;

            if (ModelState.IsValid)
            {

                var jsonData = JsonConvert.SerializeObject(kullaniciDto);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("https://localhost:44365/api/Kullanici", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Profil");

                }

            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Guncelle()
        {

            int id = HttpContext.Session.GetInt32("kullaniciId").Value;

            if (id == null)
            {
                return NotFound();
            }
            var response = await _httpClient.GetAsync("https://localhost:44365/api/Kullanici/" + id);

            KullaniciDto dto = null;

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dto = JsonConvert.DeserializeObject<KullaniciDto>(json);
            }

            return View(dto);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpGet]
        [ActionName("Profil")]
        public async Task<IActionResult> Profil()
        {
            int id = HttpContext.Session.GetInt32("kullaniciId").Value;

            if (id == null)
            {
                return NotFound();
            }
            var response = await _httpClient.GetAsync("https://localhost:44365/api/Kullanici/" + id);

            KullaniciDto dto = null;

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dto = JsonConvert.DeserializeObject<KullaniciDto>(json);
            }

            return View(dto);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string sifre, bool beniHatirla)
        {

            using (var httpClient = new HttpClient())
            {
                string url = $"https://localhost:44365/api/Kullanici/login";
                var values = new Dictionary<string, string>
                {
                    { "email", email },
                    { "sifre", sifre }
                };

                var content = new FormUrlEncodedContent(values);
                try
                {
                    var response = await httpClient.PostAsync(url, content);

                    KullaniciDto kullaniciDto = response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<KullaniciDto>(await response.Content.ReadAsStringAsync())
                : null;

                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.Mesaj = "Email veya şifre hatalı.";
                        return View();
                    }

                    // Session’a kullanıcı bilgilerini kaydet
                    HttpContext.Session.SetInt32("isAdmin", kullaniciDto.IsAdmin ? 1 : 0);
                    HttpContext.Session.SetString("adSoyad", kullaniciDto.AdSoyad ?? "");
                    HttpContext.Session.SetString("email", kullaniciDto.Email ?? "");
                    HttpContext.Session.SetInt32("kullaniciId", kullaniciDto.Id); // ➡️ Kullanıcı ID bilgisini de Session'a kaydediyoruz

                    // ✅ 4. Başarılı giriş olursa LocalStorage'a ID aktaracağız (Login.cshtml dosyasında)
                    TempData["kullaniciId"] = kullaniciDto.Id;
                }catch(Exception ex)
                {

                }
                }

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
