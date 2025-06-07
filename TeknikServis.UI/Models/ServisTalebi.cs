using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using TeknikServis.DTO;
using TeknikServis.UI.Models.dto;

namespace TeknikServis.UI.Models
{
    public class ServisTalebi
    {
        public int Id { get; set; }


        public int KullaniciId { get; set; }

        public int? UrunId { get; set; }

        public UrunDto? Urun { get; set; }
        [ValidateNever]
        public string UrunAdi { get; set; }

        public string Aciklama { get; set; }


        public string TalepDurumu { get; set; } = "Oluşturuldu";

        public DateTime TalepTarihi { get; set; } = DateTime.Now;

        public bool Durum { get; set; } = false; // false = Bekliyor, true = Çözüldü

        public virtual Kullanici? Kullanici { get; set; }

        public List<string> TalepResimleri { get; set; }
    }
}
