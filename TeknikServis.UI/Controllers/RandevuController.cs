using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using TeknikServis.DTO;

namespace TeknikServis.UI.Controllers
{
    public class RandevuController : Controller
    {
        private readonly HttpClient _httpClient;

        public RandevuController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44365/"); 
        }

        public async Task<IActionResult> Index()
        {
            string servisAdresi = "api/Randevu";

            if (HttpContext.Session.GetInt32("isAdmin") == 0)
            {
                servisAdresi += "/randevularByKullaniciId?id=" + HttpContext.Session.GetInt32("kullaniciId").Value;
            }

            var response = await _httpClient.GetAsync(servisAdresi);
            if (!response.IsSuccessStatusCode)
                return View(new List<RandevuDto>());

            var json = await response.Content.ReadAsStringAsync();
            var randevular = JsonConvert.DeserializeObject<List<RandevuDto>>(json);

            return View(randevular);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id, string musteriAdi, String urunAdi)
        {
            ViewBag.MusteriAdi =musteriAdi;
            ViewBag.UrunAdi = urunAdi;
            ViewBag.ServisTalebiId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RandevuDto dto)
        {

            string servisAdresi = $"api/Randevu/tarihlerandevu?tarih={Uri.EscapeDataString(dto.Tarihi.ToString("yyyy-MM-ddTHH:mm:ss"))}";

          
            var response1 = await _httpClient.GetAsync(servisAdresi);
            if (!response1.IsSuccessStatusCode)
                return View();

            var json = await response1.Content.ReadAsStringAsync();
            var randevu = JsonConvert.DeserializeObject<RandevuDto>(json);

            if(randevu != null)
            {
                ViewBag.MusteriAdi = randevu.ServisTalebi.Kullanici.AdSoyad;
                ViewBag.UrunAdi = randevu.ServisTalebi.Urun.Ad;
                ViewBag.ServisTalebiId = randevu.ServisTalebiId;
                ModelState.AddModelError("", "Bu tarihte zaten bir randevu var. Lütfen farklı bir tarih seçin.");
                return View(dto);
            }

            dto.RandevuDurumu = RandevuDurumu.PLANLANDI;
            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Randevu", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Ekleme başarısız");
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/Randevu/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<RandevuDto>(json);

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RandevuDto dto)
        {
            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Randevu/{dto.Id}", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Güncelleme başarısız");
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Randevu/{id}");
            return RedirectToAction("Index");
        }
    }
}
