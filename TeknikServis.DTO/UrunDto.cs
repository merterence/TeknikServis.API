
using System.ComponentModel;

namespace TeknikServis.DTO
{

    public class UrunDto
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public Kategori Kategorisi { get; set; }
        public string Aciklama { get; set; }

        public decimal MinServisUcreti { get; set; }

        public decimal MaxServisUcreti { get; set; }

    }


   
}