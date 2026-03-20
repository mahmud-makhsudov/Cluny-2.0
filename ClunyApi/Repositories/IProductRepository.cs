using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public interface IProductRepository
    {
        Task AddOptionGroupToProductAsync(int productId, int optionGroupId);
        Task<Product> CreateAsync(CreateProductDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync(string? filter);
        Task<IEnumerable<Product>> GetByCategoryAsync(int id);
        Task<Product> GetByIdAsync(int id);
        Task RemoveOptionGroupFromProductAsync(int productId, int optionGroupId);
        Task UpdateAsync(int id, UpdateProductDto dto);
    }
}