using LearnNet_CatalogService.Data.Entities;
using Microsoft.EntityFrameworkCore;

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
                .HasKey(c => c.Id);
            modelBuilder.Entity<Category>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Category>()
                .Property(b => b.Name)
                    .IsRequired()
                    .HasMaxLength(50);

            modelBuilder.Entity<Product>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Product>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

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
