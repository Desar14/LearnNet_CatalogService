using LearnNet_CatalogService.Data.Common;

namespace LearnNet_CatalogService.Data.Entities
{
    public class Category : BaseAuditableEntity
    {
        public required string Name { get; set; }
        public Uri? ImageUrl { get; set; }
        public Category? ParentCategory { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
