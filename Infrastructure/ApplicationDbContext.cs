using LearnNet_CatalogService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace LearnNet_CatalogService.DataAccessSQL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(b => b.Name)
                    .IsRequired()
                    .HasMaxLength(50);

            modelBuilder.Entity<Product>()
                .Property(b =>b.Name)
                    .IsRequired()
                    .HasMaxLength(50);

            modelBuilder.Entity<Product>()
                .HasOne(e => e.Category);

            modelBuilder.Entity<Product>()
                .Property(b => b.Price)
                    .HasPrecision(14,2)
                    .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(b => b.Amount)
                    .IsRequired();
        }
    }
}
