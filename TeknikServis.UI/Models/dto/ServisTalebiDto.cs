namespace TeknikServis.UI.Models.dto
{
    public class ServisTalebiDto
    {
        public int Id { get; set; } // Silme için şart

        public string? UrunAdi { get; set; }

        public string? Aciklama { get; set; }

        public string? AdSoyad { get; set; }

        public string? Email { get; set; }

        public string? Adres { get; set; }

        public string? TalepDurumu { get; set; } = "Oluşturuldu"; // Varsayılan değer
    }
}
