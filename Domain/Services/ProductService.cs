using FluentValidation;
using LearnNet_CatalogService.Core.DTO;
using LearnNet_CatalogService.Core.Interfaces;
using LearnNet_CatalogService.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnNet_CatalogService.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly ILogger<ProductService> _logger;
        private readonly IValidator<Product> _categoryValidator;

        public ProductService(IRepository<Product> repository,
                              ILogger<ProductService> logger,
                              IValidator<Product> categoryValidator)
        {
            _repository = repository;
            _logger = logger;
            _categoryValidator = categoryValidator;
        }

        public async Task<bool> AddProductAsync(ProductDTO dto)
        {
            var entity = ProductDTO.MapTo(dto) ?? throw new ArgumentNullException(nameof(dto));

            var validationResult = _categoryValidator.Validate(entity);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _repository.Add(entity);

            var result = await _repository.Commit();

            return result > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            await _repository.Delete(productId);

            var result = await _repository.Commit();

            return result > 0;
        }

        public async Task<IList<ProductDTO>> GetAllProductsAsync()
        {
            var entities = await _repository.GetAll();

            var result = entities.Select(ProductDTO.MapFrom).ToList();

            return result;
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);

            return ProductDTO.MapFrom(entity);
        }

        public async Task<bool> UpdateProductAsync(ProductDTO dto)
        {
            var entity = ProductDTO.MapTo(dto) ?? throw new ArgumentNullException(nameof(dto));

            var validationResult = _categoryValidator.Validate(entity);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _repository.Update(entity);

            var result = await _repository.Commit();

            return result > 0;
        }
    }
}
