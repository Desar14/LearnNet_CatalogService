using LearnNet_CatalogService.Core.DTO;

namespace LearnNet_CatalogService.Api.Models.Category
{
    public class CategoryWriteModel
    {
        public required string Name { get; set; }
        public Uri? ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }

        public static CategoryDTO MapTo(CategoryWriteModel model)
        {
            var dto = new CategoryDTO
            {
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                ParentCategoryId = model.ParentCategoryId
            };

            return dto;
        }
    }
}
