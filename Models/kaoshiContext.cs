using Microsoft.EntityFrameworkCore;
namespace kaoshi.Models
{
    public class kaoshiContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public kaoshiContext(DbContextOptions<kaoshiContext> options) : base(options) { }
        public DbSet<users> users { get; set; }
        public DbSet<events> events { get; set; }
        public DbSet<joins> joins { get; set; }
        
    }
}