using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknikServis.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeknikServis.API.Services;
using TeknikServis.DTO;
using Microsoft.AspNetCore.SignalR;

namespace TeknikServis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServisTalebiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ServisTalebiController(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;

        }

        // 1️⃣ Tüm talepleri getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServisTalebi>>> GetTalepler()
        {
            return await _context.ServisTalepleri
                .Include(t => t.Kullanici) // Kullanıcı bilgilerini de getiriyoruz
                .Include(t => t.Urun)
                .ToListAsync();
        }

        // ✅ Kullanıcının kendi taleplerini getir 

        [HttpGet("kullanici")]
        public async Task<ActionResult<IEnumerable<ServisTalebi>>> GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email bilgisi gerekli.");

            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(k => k.Email == email);
            if (kullanici == null)
                return NotFound("Kullanıcı bulunamadı.");

            var talepler = await _context.ServisTalepleri
                .Where(t => t.KullaniciId == kullanici.Id)
                .Include(t => t.Kullanici)
                .ToListAsync();

            return talepler;
        }

        // 🆕 KullaniciId ile filtreleme
        [HttpGet("kullanici-id")]
        public async Task<ActionResult<IEnumerable<ServisTalebi>>> GetByKullaniciId(int id)
        {
            var talepler = await _context.ServisTalepleri
                .Where(t => t.KullaniciId == id)
                .Include(t => t.Kullanici)
                .ToListAsync();

            return talepler;
        }

        [HttpGet("KullaniciyaGoreListele/{id}")]
        public async Task<IActionResult> KullaniciyaGoreListele(int id)
        {
            var talepler = await _context.ServisTalepleri
                .Where(t => t.KullaniciId == id)
                .ToListAsync();

            var dtoList = talepler.Select(t => new ServisTalebiDto
            {
                Id = t.Id,
                UrunAdi = t.UrunAdi,
                Aciklama = t.Aciklama,
                TalepDurumu = t.TalepDurumu,
                TalepTarihi = t.TalepTarihi
              
            }).ToList();

            return Ok(dtoList);
        }


        // 2️⃣ Belirli bir talebi getir
        [HttpGet("{id}")]
        public async Task<ActionResult<ServisTalebi>> GetTalep(int id)
        {
            var talep = await _context.ServisTalepleri.Include(s => s.Urun).FirstOrDefaultAsync(s => s.Id == id);
            if (talep == null) return NotFound();

            ServisTalebiDto dto = new ServisTalebiDto
            {
                KullaniciId = talep.KullaniciId,
                UrunAdi = talep.UrunAdi,
                UrunId = talep.UrunId,
                Aciklama = talep.Aciklama,
                TalepDurumu = talep.TalepDurumu,
                TalepTarihi = talep.TalepTarihi,
                TalepResimleri = talep.TalepResimleri ?? new List<string>()
            };

            return talep;
        }

        // 3️⃣ Yeni servis talebi oluştur
        [HttpPost]
        public async Task<ActionResult<ServisTalebi>> PostTalep([FromBody] ServisTalebiDto dto)
        {
            // ❗️KullaniciId'yi dışarıdan değil, oturumdan al
            //var kullaniciIdHeader = HttpContext.Request.Headers["kullaniciId"].FirstOrDefault();

            //if (!int.TryParse(kullaniciIdHeader, out int kullaniciId))
            //{
            //    return BadRequest("Geçerli bir kullanıcı oturumu bulunamadı.");
            //}

            var talep = new ServisTalebi
            {
                KullaniciId = dto.KullaniciId,
                UrunAdi = dto.UrunAdi,
                UrunId = dto.UrunId,
                Aciklama = dto.Aciklama,
                TalepDurumu = dto.TalepDurumu ?? "Oluşturuldu",
                TalepTarihi = dto.TalepTarihi ?? DateTime.Now,
                TalepResimleri = dto.TalepResimleri ?? new List<string>()
            };

            Console.WriteLine("=== [API] Yeni Servis Talebi Geldi ===");
            Console.WriteLine($"Kullanıcı ID : {talep.KullaniciId}");
            Console.WriteLine($"Ürün Adı     : {talep.UrunAdi}");
            Console.WriteLine($"Açıklama     : {talep.Aciklama}");
            Console.WriteLine($"Talep Tarihi : {talep.TalepTarihi}");

            _context.ServisTalepleri.Add(talep);
            await _context.SaveChangesAsync();

            Kullanici kullanici = await _context.Kullanicilar.FindAsync(dto.KullaniciId);
            Urun urun = await _context.Urunler.FindAsync(talep.UrunId);

            EmailService emailService = new EmailService();
            emailService.SendEmailAsync(kullanici.Email, "Yeni Servis Talebi", "<html><body style='background-color:lightblue;'><b>Ürün Adı : </b>" + urun.Ad + "<br><b> Açıklama : </b> " + talep.Aciklama + "</body></html>");

            try
            {
                await _hubContext.Clients.All.SendAsync("YeniTalep", "YeniTalepEklendi");
            }
            catch (Exception ex)
            {

            }
           

            return CreatedAtAction(nameof(GetTalep), new { id = talep.Id }, talep);
        }


        // 4️⃣ Talebi güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTalep(int id, [FromBody] ServisTalebiDto dto)
        {
            if (id <= 0 || dto == null || id != dto.Id)
                return BadRequest("Geçersiz istek.");

            var talep = await _context.ServisTalepleri.FindAsync(id);

            if (talep == null)
                return NotFound("Talep bulunamadı.");

            // Sadece güncellenebilir alanları değiştir
            talep.Aciklama = dto.Aciklama;
            talep.TalepDurumu = dto.TalepDurumu;
            talep.UrunAdi = dto.UrunAdi;

            // Navigation property güncellenmiyor!

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // 5️⃣ Talebi sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTalep(int id)
        {
            var talep = await _context.ServisTalepleri.FindAsync(id);
            if (talep == null) return NotFound();
            _context.ServisTalepleri.Remove(talep);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
