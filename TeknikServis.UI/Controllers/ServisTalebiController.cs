using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.UI.Models;
using Newtonsoft.Json;
using TeknikServis.UI.Models.dto;
using Microsoft.AspNetCore.Http;
using TeknikServis.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using TeknikServis.UI.Helper;

namespace TeknikServis.UI.Controllers
{
    public class ServisTalebiController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient2;

        public ServisTalebiController()
        {
            _httpClient = new HttpClient();
            _httpClient2 = new HttpClient();
        }

        [HttpGet]
        public async Task<JsonResult> GetUrunlerByKategori(int id)
        {
            var response = await _httpClient2.GetAsync("https://localhost:44365/api/Urun");
            var urunler = new List<UrunDto>();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                urunler = JsonConvert.DeserializeObject<List<UrunDto>>(json);
            }

            return Json(urunler.Where(u => u.Kategorisi == (Kategori)id));
        }

        public async Task<IActionResult> YeniTalep()
        {
            

           

            ViewBag.Kategoriler = EnumHelper.GetEnumDescriptions<Kategori>();

           

            var ad = HttpContext.Session.GetString("adSoyad");
            var email = HttpContext.Session.GetString("email");

            if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Kullanici");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YeniTalep(ServisTalebi model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.TalepTarihi = DateTime.Now;

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44365/api/ServisTalebi", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Mesaj = "Servis talebiniz başarıyla oluşturuldu!";
                return RedirectToAction("KendiTaleplerim");

            }

            var response2 = await _httpClient2.GetAsync("https://localhost:44365/api/Urun");
            var urunler = new List<UrunDto>();

            if (response2.IsSuccessStatusCode)
            {
                var json = await response2.Content.ReadAsStringAsync();
                urunler = JsonConvert.DeserializeObject<List<UrunDto>>(json);
            }

            ViewData["Urunler"] = urunler.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Ad
            });
            ViewBag.Mesaj = "Talep oluşturulurken hata oluştu!";
            ViewBag.Kategoriler = EnumHelper.GetEnumDescriptions<Kategori>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:44365/api/ServisTalebi/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("KendiTaleplerim");
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
