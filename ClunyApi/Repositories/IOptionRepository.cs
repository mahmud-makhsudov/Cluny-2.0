using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public interface IOptionRepository
    {
        Task<Option> CreateAsync(CreateOptionDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<Option>> GetAllAsync();
        Task<Option> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateOptionDto dto);
    }
}