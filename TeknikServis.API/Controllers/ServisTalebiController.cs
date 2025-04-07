using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknikServis.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeknikServis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServisTalebiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServisTalebiController(AppDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Tüm talepleri getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServisTalebi>>> GetTalepler()
        {
            return await _context.ServisTalepleri.ToListAsync();
        }

        // 2️⃣ Belirli bir talebi getir
        [HttpGet("{id}")]
        public async Task<ActionResult<ServisTalebi>> GetTalep(int id)
        {
            var talep = await _context.ServisTalepleri.FindAsync(id);
            if (talep == null) return NotFound();
            return talep;
        }

        // 3️⃣ Yeni servis talebi oluştur
        [HttpPost]
        public async Task<ActionResult<ServisTalebi>> PostTalep(ServisTalebi talep)
        {
            _context.ServisTalepleri.Add(talep);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTalep), new { id = talep.Id }, talep);
        }

        // 4️⃣ Talebi güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTalep(int id, ServisTalebi talep)
        {
            if (id != talep.Id) return BadRequest();
            _context.Entry(talep).State = EntityState.Modified;
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
