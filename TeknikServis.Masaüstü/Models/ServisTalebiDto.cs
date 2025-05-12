using System;

namespace TeknikServis.Masaüstü.Models
{
    public class ServisTalebiDto
    {
        public int Id { get; set; }
        public string? UrunAdi { get; set; }
        public string? Aciklama { get; set; }
        public string? TalepDurumu { get; set; }
        public DateTime? TalepTarihi { get; set; }
        public int KullaniciId { get; set; }

        public KullaniciDto? Kullanici { get; set; } // 🆕 EKLEDİK
    }

    public class KullaniciDto
    {
        public int Id { get; set; }
        public string? AdSoyad { get; set; }
        public string? Email { get; set; }
        public string? Rol { get; set; }
        public bool IsAdmin { get; set; }


    }
}
