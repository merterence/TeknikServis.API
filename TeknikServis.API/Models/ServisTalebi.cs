using System;
using System.ComponentModel.DataAnnotations;

namespace TeknikServis.API.Models
{
    public class ServisTalebi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string KullaniciAdi { get; set; }

        [Required]
        public string UrunAdi { get; set; }

        [Required]
        public string Aciklama { get; set; }

        public DateTime TalepTarihi { get; set; } = DateTime.Now;

        public bool Durum { get; set; } = false; // false = Bekliyor, true = Çözüldü
    }
}
