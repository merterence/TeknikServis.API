namespace TeknikServis.API.Models.Dto
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using TeknikServis.DTO;
    public class ServisTalebiDto
    {
        public int Id { get; set; }


        public int KullaniciId { get; set; }

        public int? UrunId { get; set; }

        public UrunDto? Urun { get; set; }


        public string UrunAdi { get; set; }


        public string Aciklama { get; set; }


        public string TalepDurumu { get; set; } = "Oluşturuldu";

        public DateTime? TalepTarihi { get; set; } = DateTime.Now;

        public bool Durum { get; set; } = false; // false = Bekliyor, true = Çözüldü

        public virtual Kullanici? Kullanici { get; set; }
        [ValidateNever]
        public List<string>? TalepResimleri { get; set; }
    }
}
