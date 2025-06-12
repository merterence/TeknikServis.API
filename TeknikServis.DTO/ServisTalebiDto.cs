
using System.ComponentModel.DataAnnotations;

namespace TeknikServis.DTO
{
  
    public class ServisTalebiDto
    {
        public int Id { get; set; }


        public int KullaniciId { get; set; }

        public int? UrunId { get; set; }

        public UrunDto? Urun { get; set; }


        public string? UrunAdi { get; set; }

        [StringLength(500, MinimumLength =20, ErrorMessage="En az 20 en fazla 500 karakter giriniz.")]
        public string Aciklama { get; set; }


        public string TalepDurumu { get; set; } = "Oluşturuldu";

        public DateTime? TalepTarihi { get; set; } = DateTime.Now;

        public bool Durum { get; set; } = false; // false = Bekliyor, true = Çözüldü

        public  KullaniciDto? Kullanici { get; set; }
 
        public List<string>? TalepResimleri { get; set; }
    }
}
