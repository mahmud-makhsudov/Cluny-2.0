using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(string name);
        Task DeleteAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync(string? filter);
        Task<Category> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateCategoryDto dto);
    }
}