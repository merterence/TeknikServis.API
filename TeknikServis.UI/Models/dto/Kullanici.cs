namespace TeknikServis.UI.Models.dto
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string? AdSoyad { get; set; }
        public string? Email { get; set; }
        public string? Sifre { get; set; }
        public string? Rol { get; set; } = "Kullanici";
        public bool IsAdmin { get; set; }

        public virtual ICollection<ServisTalebi> ServisTalepleri { get; set; }

    }
}
