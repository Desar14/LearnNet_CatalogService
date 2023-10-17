using LearnNet_CatalogService.Core.DTO;

namespace LearnNet_CatalogService.Core.Interfaces
{
    public interface IProductService
    {
        Task<IList<ProductDTO>> GetAllProductsAsync();

        Task<ProductDTO?> GetProductByIdAsync(int id);

        Task<bool> AddProductAsync(ProductDTO dto);

        Task<bool> UpdateProductAsync(ProductDTO dto);

        Task<bool> DeleteProductAsync(int productId);
    }
}
