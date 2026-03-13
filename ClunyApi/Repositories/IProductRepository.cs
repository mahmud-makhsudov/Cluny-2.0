using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(CreateProductDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync(string? filter);
        Task<Product> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateProductDto dto);
    }
}