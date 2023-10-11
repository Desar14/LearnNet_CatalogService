using LearnNet_CatalogService.Domain.Common;

namespace LearnNet_CatalogService.Domain.Entities
{
    public class Product : BaseAuditableEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Uri? ImageUrl { get; set; }
        public required Category Category { get; set; }
        public string CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set;}
    }
}
