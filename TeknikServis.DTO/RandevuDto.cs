
namespace TeknikServis.DTO
{
    public class RandevuDto
    {
        public int Id { get; set; }

        public int ServisTalebiId{ get; set; }

        public ServisTalebiDto? ServisTalebi { get; set; }

        public DateTime Tarihi { get; set; }
        
        public RandevuDurumu? RandevuDurumu { get; set; }


    }
}
