using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.UI.Models;
using Newtonsoft.Json;
using TeknikServis.UI.Models.dto;

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
    }
}
