namespace TeknikServis.UI.Models
{
    public class ServisTalebi
    {
        public int Id { get; set; }  // ID otomatik atanmalı, bu yüzden formda göndermeyeceğiz
        public string UrunAdi { get; set; }
        public string Aciklama { get; set; }
        public string KullaniciAdi { get; set; }
    }
}
