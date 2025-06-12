using Microsoft.EntityFrameworkCore;
using TeknikServis.DTO;

namespace TeknikServis.API.Models
{
    public class Urun
    {
        public int Id { get; set; }
        public  string Ad { get; set; }
        public Kategori Kategorisi { get; set; }

        public string Aciklama { get; set; }
        [Precision(18, 2)]
        public decimal MinServisUcreti { get; set; }
        [Precision(18, 2)] 
        public decimal MaxServisUcreti { get; set; }

    }
}
