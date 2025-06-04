using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TeknikServis.DTO;
using TeknikServis.UI.Models.dto;

namespace TeknikServis.UI.Controllers
{
    public class UrunController : Controller
    {
        private readonly HttpClient _httpClient;

        public UrunController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44365/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("api/Urun");
            var urunler = new List<UrunDto>();

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                urunler = JsonConvert.DeserializeObject<List<UrunDto>>(json);
            }

            return View(urunler);
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(UrunDto urun)
        {
            
            var jsonContent = new StringContent(JsonConvert.SerializeObject(urun), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Urun", jsonContent);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Sil(int id)
        {
            await _httpClient.DeleteAsync($"api/Urun/{id}");
            return RedirectToAction("Index");
        }
    }
}
