using Microsoft.EntityFrameworkCore;

namespace TeknikServis.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ServisTalebi> ServisTalepleri { get; set; } // Var olan tablo
        public DbSet<Kullanici> Kullanicilar { get; set; }        // Yeni eklenen tablo ✅

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
