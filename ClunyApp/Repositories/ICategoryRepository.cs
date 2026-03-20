using Shared.Models;

namespace ClunyApp.Repositories
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<Category?> GetAsync(int id);
        Task<List<Category>> GetAllAsync(string? filter = null);
        Task<bool> UpdateAsync(Category? category);
    }
}