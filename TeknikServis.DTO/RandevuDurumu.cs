using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.DTO
{
   public  enum RandevuDurumu
    {

        [Description("Planlandı")]
        PLANLANDI = 1,
        [Description("Red edildi")]
        REDEDILDI,
        [Description("Onaylandı")]
        ONAYLANDI,
        [Description("Tamamlandı")]
        TAMAMLANDI,
        [Description("İptal edildi")]
        IPTALEDILDI = 1,

    }
}
