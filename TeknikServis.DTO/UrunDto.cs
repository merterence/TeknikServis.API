
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

        ELTEMINALI, BARKODOKUYUCU, YAZICI, TELEFON, TABLET, LAPTOP, MONITOR, KULAKLIK, KLAVYE, MOUSE, DIGER
    }
}