
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
        [Description("Barkod Okuyucular")]
        BARKODOKUYUCULAR=1,
        [Description("Mobil Terminaller")]
        MOBILTERMINALLER,
        [Description("Barkod Yazıcılar")]
        BARKODYAZICILAR,
        [Description("Endüstriyel Machine Vision ve Sabit Okuyucular")]
        SABITOKUYUCULAR,
        [Description("RF-ID Ürünleri")]
        RF_IDURUNLERI,
        [Description("Dayanıklı Bilgisayar")]
        DAYANIKLIBILGISAYAR,
        [Description("Endüstriyel PC & Tablet")]
        ENDUSTRIYELPC_TABLET,
        [Description("Kablosuz Ağ Ürünleri")]
        KABLOSUZAGURUNLERI,
        [Description("Sarf Malzemeleri")]
        SARFMALZEMELERI,
        [Description("Araca Monte Bilgisayarlar")]
        ARACAMONTEBILGISAYARLAR

    }
}