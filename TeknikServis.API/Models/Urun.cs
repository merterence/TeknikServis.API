using TeknikServis.DTO;

namespace TeknikServis.API.Models
{
    public class Urun
    {
        public int Id { get; set; }
        public  string Ad { get; set; }
        public Kategori Kategorisi { get; set; }

        public string Aciklama { get; set; }
 
    }
}
