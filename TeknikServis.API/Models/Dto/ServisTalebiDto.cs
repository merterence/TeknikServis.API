namespace TeknikServis.API.Models.Dto
{
    public class ServisTalebiDto
    {
        public int Id { get; set; } // 🔥 Güncelleme için şart
        public string UrunAdi { get; set; }
        public string Aciklama { get; set; }
        public int KullaniciId { get; set; }

        // Opsiyonel alanlar (istersen):
        public string? Adres { get; set; }
        public string? TalepDurumu { get; set; } = "Oluşturuldu";
        public DateTime? TalepTarihi { get; set; }
    }
}
