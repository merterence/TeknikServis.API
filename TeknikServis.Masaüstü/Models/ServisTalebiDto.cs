using System;

namespace TeknikServis.Masaüstü.Models
{
    public class ServisTalebiDto
    {
        public int Id { get; set; }
        public string? UrunAdi { get; set; }
        public string? Aciklama { get; set; }
        public string? KullaniciAdi { get; set; }
        public string? Email { get; set; }
        public string? Adres { get; set; }
        public string? TalepDurumu { get; set; }
        public DateTime? TalepTarihi { get; set; }
    }
}