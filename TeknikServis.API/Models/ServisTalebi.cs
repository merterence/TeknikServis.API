using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace TeknikServis.API.Models
{
    public class ServisTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int KullaniciId { get; set; }

        public int? UrunId { get; set; }

        public Urun? Urun { get; set; }

        public string? UrunAdi { get; set; }

        [Required]
        public string Aciklama { get; set; }


        public string TalepDurumu { get; set; } = "Oluşturuldu";

        public DateTime TalepTarihi { get; set; } = DateTime.Now;

        public bool Durum { get; set; } = false; // false = Bekliyor, true = Çözüldü

        public virtual Kullanici? Kullanici { get; set; }
        [ValidateNever]
        public List<string>? TalepResimleri { get; set; }
    }
}
