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
    }
}
