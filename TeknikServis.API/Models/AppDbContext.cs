using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeknikServis.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ServisTalebi> ServisTalepleri { get; set; } // Var olan tablo
        public DbSet<Kullanici> Kullanicilar { get; set; }        // Yeni eklenen tablo ✅
        public DbSet<Urun> Urunler { get; set; } // Yeni eklenen tablo ✅

        public DbSet<Adres> Adresler { get; set; } // Yeni eklenen tablo ✅

        public DbSet<Randevu> Randevular { get; set; }

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
