﻿using LearnNet_CatalogService.Core.DTO;
using LearnNet_CatalogService.HATEOAS;

namespace LearnNet_CatalogService.Models.Product
{
    public class ProductWriteModel
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Uri? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }

        public static ProductDTO MapTo(ProductWriteModel model)
        {
            var dto = new ProductDTO
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                CategoryId = 0,
                Price = model.Price,
                Amount = model.Amount
            };

            return dto;
        }
    }
}