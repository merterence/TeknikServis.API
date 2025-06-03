using TeknikServis.DTO;

namespace TeknikServis.UI.Models
{
    public class ServisTalebi
    {
        public int Id { get; set; }  // ID otomatik atanmalı, bu yüzden formda göndermeyeceğiz

        public string? UrunAdi { get; set; }

        public int UrunId { get; set; }

        public string? Aciklama { get; set; }

        public string? KullaniciAdi { get; set; }

        public string? AdSoyad { get; set; }  // Session’dan otomatik alınan isim

        public string? Email { get; set; }    // Giriş yapan kullanıcının emaili

        public string? Adres { get; set; }    // İleride kullanıcıdan alınacak

        public string? TalepDurumu { get; set; } = "Oluşturuldu";  // Varsayılan

        Deneme Deneme { get; set; } = new Deneme(); // Örnek DTO, ileride kullanılabilir
    }
}
