using LearnNet_CatalogService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnNet_CatalogService.DataAccessSQL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category<int>> Categories { get; set; }
        public DbSet<Product<int>> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category<int>>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Category<int>>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Category<int>>()
                .Property(b => b.Name)
                    .IsRequired()
                    .HasMaxLength(50);

            modelBuilder.Entity<Product<int>>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Product<int>>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Product<int>>()
                .Property(b =>b.Name)
                    .IsRequired()
                    .HasMaxLength(50);

            modelBuilder.Entity<Product<int>>()
                .HasOne(e => e.Category);

            modelBuilder.Entity<Product<int>>()
                .Property(b => b.Price)
                    .HasPrecision(14,2)
                    .IsRequired();

            modelBuilder.Entity<Product<int>>()
                .Property(b => b.Amount)
                    .IsRequired();
        }
    }
}
