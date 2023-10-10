using LearnNet_CatalogService.Domain.Common;

namespace LearnNet_CatalogService.Domain.Entities
{
    public class Category : BaseAuditableEntity
    {
        public required string Name { get; set; }
        public Uri? ImageUrl { get; set; }
        public Category? ParentCategory { get; set; }
    }
}
