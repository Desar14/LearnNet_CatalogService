using LearnNet_CatalogService.Core.DTO;

namespace LearnNet_CatalogService.Api.Models.Product
{
    public class ProductWriteModel
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Uri? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }

        public static ProductDTO MapTo(ProductWriteModel model)
        {
            var dto = new ProductDTO
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                CategoryId = model.CategoryId,
                Price = model.Price,
                Amount = model.Amount
            };

            return dto;
        }
    }
}
