using Microsoft.AspNetCore.Mvc;
using TeknikServis.API.Models;
using Microsoft.EntityFrameworkCore;

namespace TeknikServis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrunController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UrunController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Urun>>> GetUrunler()
        {
            return await _context.Urunler.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Urun>> PostUrun(Urun urun)
        {
            _context.Urunler.Add(urun);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUrunler", new { id = urun.Id }, urun);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrun(int id)
        {
            var urun = await _context.Urunler.FindAsync(id);
            if (urun == null)
                return NotFound();

            _context.Urunler.Remove(urun);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
