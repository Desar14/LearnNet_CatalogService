using LearnNet_CatalogService.Domain.Entities;

namespace LearnNet_CatalogService.Core.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Uri? ImageUrl { get; set; }
        public required string CategoryId { get; set; }
        public CategoryDTO Category { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }

        public static Product? MapTo(ProductDTO dto)
        {
            var entity = new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                Category = CategoryDTO.MapTo(dto.Category),
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Amount = dto.Amount
            };

            return entity;
        }

        public static ProductDTO MapFrom(Product entity)
        {
            var dto = new ProductDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                CategoryId = entity.CategoryId,
                Category = CategoryDTO.MapFrom(entity.Category),
                Price = entity.Price,
                Amount = entity.Amount
            };
            return dto;
        }
    }
}
