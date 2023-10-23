using LearnNet_CatalogService.Data.Common;

namespace LearnNet_CatalogService.Data.Entities
{
    public class Category<TKey> : BaseAuditableEntity<TKey> where TKey : struct
    {
        public required string Name { get; set; }
        public Uri? ImageUrl { get; set; }
        public Category<TKey>? ParentCategory { get; set; }
        public TKey? ParentCategoryId { get; set; }
    }
}
