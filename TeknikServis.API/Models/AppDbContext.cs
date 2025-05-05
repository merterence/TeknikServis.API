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
            modelBuilder.Entity<ServisTalebi>()
            .HasOne(t => t.Kullanici)
            .WithMany(k => k.ServisTalepleri)
            .HasForeignKey(t => t.KullaniciId)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
