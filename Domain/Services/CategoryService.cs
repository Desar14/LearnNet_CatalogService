using FluentValidation;
using LearnNet_CatalogService.Core.DTO;
using LearnNet_CatalogService.Core.Interfaces;
using LearnNet_CatalogService.Data.Entities;
using Microsoft.Extensions.Logging;

namespace LearnNet_CatalogService.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repository;
        private readonly ILogger<CategoryService> _logger;
        private readonly IValidator<Category> _categoryValidator;

        public CategoryService(IRepository<Category> repository,
                               ILogger<CategoryService> logger,
                               IValidator<Category> categoryValidator)
        {
            _repository = repository;
            _logger = logger;
            _categoryValidator = categoryValidator;
        }

        public async Task<bool> AddCategoryAsync(CategoryDTO dto)
        {
            var entity = CategoryDTO.MapTo(dto) ?? throw new ArgumentNullException(nameof(dto));
            
            var validationResult = _categoryValidator.Validate(entity);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _repository.Add(entity);

            var result = await _repository.Commit();

            return result > 0;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            await _repository.Delete(categoryId);

            var result = await _repository.Commit();

            return result > 0;
        }

        public async Task<IList<CategoryDTO>> GetAllCategoriesAsync()
        {
            var entities = await _repository.GetAll();

            var result = entities.Select(CategoryDTO.MapFrom).ToList();

            return result;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            var entity = await _repository.GetById(id);

            return CategoryDTO.MapFrom(entity);
        }

        public async Task<bool> UpdateCategoryAsync(CategoryDTO dto)
        {
            var entity = CategoryDTO.MapTo(dto) ?? throw new ArgumentNullException(nameof(dto));

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
