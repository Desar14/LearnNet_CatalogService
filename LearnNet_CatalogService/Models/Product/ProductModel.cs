using LearnNet_CatalogService.Api.Models.HATEOAS;
using LearnNet_CatalogService.Core.DTO;

namespace LearnNet_CatalogService.Api.Models.Product
{
    public class ProductModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Uri? ImageUrl { get; set; }
        public required int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }

        public IList<Link> Links { get; set; }

        public static ProductModel MapFrom(ProductDTO dto)
        {
            var model = new ProductModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                Amount = dto.Amount
            };

            return model;
        }
    }
}
