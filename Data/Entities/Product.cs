using LearnNet_CatalogService.Data.Common;

namespace LearnNet_CatalogService.Data.Entities
{
    public class Product<TKey> : BaseAuditableEntity<TKey> where TKey : struct
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Uri? ImageUrl { get; set; }
        public required Category<TKey> Category { get; set; }
        public TKey CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
