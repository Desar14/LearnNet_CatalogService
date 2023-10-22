using LearnNet_CatalogService.Core.DTO;

namespace LearnNet_CatalogService.Core.Interfaces
{
    public interface IProductService
    {
        Task<IList<ProductDTO>> GetAllProductsAsync();

        Task<IList<ProductDTO>> GetAllProductsByCategoryIdAsync(int categoryId, int page = 0, int limit = 50);

        Task<ProductDTO?> GetProductByIdAsync(int id);

        Task<int> AddProductAsync(ProductDTO dto);

        Task<bool> UpdateProductAsync(ProductDTO dto);

        Task<bool> DeleteProductAsync(int productId);
    }
}
