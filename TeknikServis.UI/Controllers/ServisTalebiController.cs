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
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Reflection;

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
        public async Task<IActionResult> TalepDetay(int id)
        {

            var response = await _httpClient.GetAsync("https://localhost:44365/api/ServisTalebi/"+id);

            ServisTalebiDto dto = null;

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dto = JsonConvert.DeserializeObject<ServisTalebiDto>(json);
            }

            return View(dto);
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
        [HttpGet]
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
        public async Task<IActionResult> YeniTalep(ServisTalebiDto model, List<IFormFile> TalepResimleri)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategoriler = EnumHelper.GetEnumDescriptions<Kategori>();

                var ad = HttpContext.Session.GetString("adSoyad");
                var email = HttpContext.Session.GetString("email");
                return View();
            }
                

            if(TalepResimleri != null && TalepResimleri.Count > 0)
            {
                foreach(var resim in TalepResimleri)
                {
                    var upload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/talep_resim");

                    var extension = Path.GetExtension(resim.FileName); //resim.png
                    var fileName = Guid.NewGuid().ToString() + extension; //dslfdslkfhsıy42r40389753.png
                    var filePath = Path.Combine(upload, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await resim.CopyToAsync(stream);
                    }

                    model.TalepResimleri.Add(fileName);

                }
            }


            model.TalepTarihi = DateTime.Now;

            model.KullaniciId = HttpContext.Session.GetInt32("kullaniciId").Value;

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
        [HttpGet]
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
