using LearnNet_CatalogService.Core.DTO;
using LearnNet_CatalogService.HATEOAS;

namespace LearnNet_CatalogService.Models.Category
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public Uri? ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }
        public IList<Link> Links {get; set; }

        public static CategoryModel MapFrom(CategoryDTO dto)
        {
            var model = new CategoryModel
            {
                Id = dto.Id,
                Name = dto.Name,
                ImageUrl = dto.ImageUrl,
                ParentCategoryId = dto.ParentCategoryId
            };

            return model;
        }

        public static CategoryDTO MapTo(CategoryModel model)
        {
            var dto = new CategoryDTO
            {
                Id = model.Id,
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                ParentCategoryId = model.ParentCategoryId
            };

            return dto;
        }
    }
}
