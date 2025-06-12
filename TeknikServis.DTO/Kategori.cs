using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.DTO
{
    public enum Kategori
    {
        [Description("Barkod Okuyucular")]
        BARKODOKUYUCULAR = 1,
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
