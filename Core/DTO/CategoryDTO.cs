using LearnNet_CatalogService.Domain.Entities;

namespace LearnNet_CatalogService.Core.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public Uri? ImageUrl { get; set; }

        public CategoryDTO? ParentCategory { get; set; }
        public int ParentCategoryId { get; set; }

        public static Category? MapTo(CategoryDTO? dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                ImageUrl = dto.ImageUrl,
                ParentCategoryId = dto.ParentCategoryId,
                ParentCategory = MapTo(dto.ParentCategory)
            };

            return entity;
        }

        public static CategoryDTO? MapFrom(Category? entity) 
        {
            if (entity == null)
            {
                return null;
            }

            var dto = new CategoryDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                ImageUrl = entity.ImageUrl,
                ParentCategoryId = entity.ParentCategoryId,
                ParentCategory = MapFrom(entity.ParentCategory)
            };

            return dto;
        }
    }
}
