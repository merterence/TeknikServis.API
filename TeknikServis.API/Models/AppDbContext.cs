using Microsoft.EntityFrameworkCore;

namespace TeknikServis.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ServisTalebi> ServisTalepleri { get; set; } // Buraya ekledik

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
