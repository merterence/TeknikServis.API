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

            var response = await _httpClient.PostAsync("https://localhost:5001/api/ServisTalebi", content);

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

        // ✅ Giriş yapan kullanıcının sadece kendi taleplerini gösteren action
        public async Task<IActionResult> KendiTaleplerim()
        {
            var email = HttpContext.Session.GetString("email");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Kullanici");
            }

            List<ServisTalebi> kullaniciTalepleri = new List<ServisTalebi>();

            var response = await _httpClient.GetAsync("https://localhost:44365/api/ServisTalebi");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var tumTalepler = JsonConvert.DeserializeObject<List<ServisTalebi>>(json);

                kullaniciTalepleri = tumTalepler
                    .Where(t => t.Email == email)
                    .ToList();
            }

            return View(kullaniciTalepleri);
        }
    }
}
