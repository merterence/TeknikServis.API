using System.ComponentModel.DataAnnotations;

namespace TeknikServis.API.Models
{
    public class Adres
    {
        [Key]
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }

        public string Sehir { get; set; }

        public string Ilce { get; set; }

        public string Mahalle { get; set; }

        public string Sokak { get; set; }

        public string No { get; set; }
    }
}