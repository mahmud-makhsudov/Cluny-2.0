using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public interface IOrderRepository
    {
        Task AddSelectedOptionToOrderAsync(int orderId, int optionId);
        Task<Order> CreateAsync(CreateOrderDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync(string? filter);
        Task<Order> GetByIdAsync(int id);
        Task<Order> GetByUserAsync(string userId);
        Task RemoveSelectedOptionFromOrderAsync(int orderId, int optionId);
        Task UpdateAsync(int id, UpdateOrderDto dto);
    }
}