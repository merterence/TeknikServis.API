using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.DTO
{
    public class KullaniciDto
    {
        public int Id { get; set; }
        public string? AdSoyad { get; set; }
        public string? Email { get; set; }
        public string? Sifre { get; set; }
        public string? Rol { get; set; } 
        public bool IsAdmin { get; set; }
        public AdresDto? AdresDto { get; set; }


    }
}
