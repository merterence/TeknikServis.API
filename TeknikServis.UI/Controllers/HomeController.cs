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
        // sesiondaki kullaniciId null ise kullanýcý/login sayfasýna yönlendir

        if (HttpContext.Session.GetInt32("kullaniciId") == null)
        {
            // http://dfgdfgdfg.kullanýcý/login url sine yönlendir
            //return RedirectToAction("Login", "Kullanici");

            return Redirect("Kullanici/login");


        }

        List<ServisTalebiDto> servisTalepleri = new();

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://localhost:44365/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/ServisTalebi");

            if (response.IsSuccessStatusCode)
            {
                string jsonResult = await response.Content.ReadAsStringAsync();
                servisTalepleri = JsonConvert.DeserializeObject<List<ServisTalebiDto>>(jsonResult);
            }
        }

        return View(servisTalepleri);
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
