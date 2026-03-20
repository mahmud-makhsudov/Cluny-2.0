using Shared.Models;

namespace ClunyApp.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product, IFormFile? imageFile = null);
        Task AddOptionToProduct(int productId, int optionGroupId);
        Task<bool> DeleteAsync(int id);
        Task<List<Product>> GetAllAsync(string? filter = null);
        Task<List<Product>> GetByCategoryAsync(int id);
        Task<Product?> GetByIdAsync(int id);
        Task RemoveOptionFromProduct(int productId, int optionGroupId);
        Task<bool> UpdateAsync(Product product, IFormFile? imageFile = null);
    }
}