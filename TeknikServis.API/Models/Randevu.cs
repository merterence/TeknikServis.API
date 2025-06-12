using TeknikServis.DTO;

namespace TeknikServis.API.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        public int ServisTalebiId{ get; set; }

        public ServisTalebi ServisTalebi { get; set; }

        public DateTime Tarihi { get; set; }

        public RandevuDurumu RandevuDurumu { get; set; }


    }
}
