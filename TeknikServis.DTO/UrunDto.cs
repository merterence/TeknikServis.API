
using System.ComponentModel;

namespace TeknikServis.DTO
{

    public class UrunDto
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public Kategori Kategorisi { get; set; }
        public string Aciklama { get; set; }


    }


    public enum Kategori
    {
        [Description("El Terminali")]
        ELTERMINALI,
        [Description("Barkod Okuyucu")]
        BARKODOKUYUCU,
        [Description("Yazıcı")]
        YAZICI
    }
}