using LearnNet_CatalogService.Data.Common;

namespace LearnNet_CatalogService.Data.Entities
{
    public class Product : BaseAuditableEntity<int>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Uri? ImageUrl { get; set; }
        public required Category Category { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
