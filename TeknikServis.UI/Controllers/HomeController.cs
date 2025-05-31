using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TeknikServis.UI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using TeknikServis.UI.Models.dto;
using Newtonsoft.Json;

namespace TeknikServis.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var kullaniciId = HttpContext.Session.GetInt32("kullaniciId");

        if (kullaniciId == null)
        {
            return RedirectToAction("Login", "Kullanici");
        }

        List<ServisTalebiDto> servisTalepleri = new();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://localhost:44365/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // ?? Sadece kullanýcýnýn taleplerini getiriyoruz
            HttpResponseMessage response = await client.GetAsync($"api/ServisTalebi/KullaniciyaGoreListele/{kullaniciId}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                servisTalepleri = JsonConvert.DeserializeObject<List<ServisTalebiDto>>(jsonResult);
            }
        }

        return View(servisTalepleri);
    }
    public IActionResult Iletisim()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
