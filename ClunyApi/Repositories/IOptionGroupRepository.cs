using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public interface IOptionGroupRepository
    {
        Task<OptionGroup> CreateAsync(CreateOptionGroupDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<OptionGroup>> GetAllAsync(string? filter);
        Task<OptionGroup> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateOptionGroupDto dto);
    }
}