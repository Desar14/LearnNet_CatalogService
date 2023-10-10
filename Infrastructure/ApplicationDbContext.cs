using LearnNet_CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnNet_CatalogService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
    }
}
