using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.UI.Models;
using Newtonsoft.Json;
using TeknikServis.UI.Models.dto;
using Microsoft.AspNetCore.Http;

namespace TeknikServis.UI.Controllers
{
    public class ServisTalebiController : Controller
    {
        private readonly HttpClient _httpClient;

        public ServisTalebiController()
        {
            _httpClient = new HttpClient();
        }

        public IActionResult YeniTalep()
        {
            var ad = HttpContext.Session.GetString("adSoyad");
            var email = HttpContext.Session.GetString("email");

            if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Kullanici");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YeniTalep(ServisTalebiDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44365/api/ServisTalebi", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Mesaj = "Servis talebiniz başarıyla oluşturuldu!";
                return View();
            }

            ViewBag.Mesaj = "Talep oluşturulurken hata oluştu!";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:44365/api/ServisTalebi/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["SilmeHatasi"] = "Silme işlemi başarısız oldu!";
                return RedirectToAction("Index", "Home");
            }
        }

        // ✅ Kullanıcının sadece kendi taleplerini gösteren kısım
        public async Task<IActionResult> KendiTaleplerim()
        {
            var kullaniciId = HttpContext.Session.GetInt32("kullaniciId");

            if (kullaniciId == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            List<ServisTalebiDto> kullaniciTalepleri = new List<ServisTalebiDto>();

            var response = await _httpClient.GetAsync("https://localhost:44365/api/ServisTalebi");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var tumTalepler = JsonConvert.DeserializeObject<List<ServisTalebiDto>>(json);

                kullaniciTalepleri = tumTalepler
                     .Where(t => t.KullaniciId == kullaniciId.Value)
                    .ToList();
            }

            return View(kullaniciTalepleri);
        }
    }
}
