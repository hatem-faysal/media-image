using Microsoft.EntityFrameworkCore;
using image.Models;

namespace image.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>().ToTable("products");
            builder.Entity<Media>().ToTable("media");
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Media> Medias { get; set; }


    }
}
