using LearnNet_CatalogService.Core.DTO;

namespace LearnNet_CatalogService.Core.Interfaces
{
    public interface IMessagePublisher 
    {
        Task<bool> PublishUpdateMessage(ProductDTO dto);
    }
}
