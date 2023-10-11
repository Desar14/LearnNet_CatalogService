using LearnNet_CatalogService.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnNet_CatalogService.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IList<CategoryDTO>> GetAllCategoriesAsync();

        Task<CategoryDTO> GetCategoryByIdAsync(int id);

        Task<bool> AddCategoryAsync(CategoryDTO dto);

        Task<bool> UpdateCategoryAsync(CategoryDTO dto);

        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
