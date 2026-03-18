using AutoMapper;
using ClunyApi.Data;
using ClunyApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public OrderRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<Order>> GetAllAsync(string? filter)
        {
            var query = context.Orders
                .Include(x => x.Product)
                .Include(x => x.User)
                 .Include(x => x.SelectedOptions)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var normalized = filter.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.Product.Name, $"%{normalized}%") ||
                    EF.Functions.Like((x.Product.Description ?? string.Empty), $"%{normalized}%") ||
                    EF.Functions.Like((x.User.PhoneNumber ?? string.Empty), $"%{normalized}%") ||
                    EF.Functions.Like((x.User.Email ?? string.Empty), $"%{normalized}%"));
            }

            return await query.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var order = await context.Orders
                .Include(x => x.Product)
                .Include(x => x.User)
                .Include(x => x.SelectedOptions)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (order == null) throw new EntityNotFoundException("Order", id);

            return order;
        }

        public async Task<Order> GetByUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId is required.", nameof(userId));

            var user = await context.Users.FindAsync(userId);
            if (user == null) throw new EntityNotFoundException("User", userId);

            var order = await context.Orders
                .Include(x => x.Product)
                .Include(x => x.User)
                .Include(x => x.SelectedOptions)
                .Where(x => x.User.Id == userId)
                .FirstOrDefaultAsync();

            if (order == null) throw new EntityNotFoundException("Order", userId);

            return order;
        }

        public async Task<Order> CreateAsync(CreateOrderDto dto)
        {
            if (dto.ProductId < 0) throw new ArgumentOutOfRangeException(nameof(dto.ProductId), "Product Id must be a non-negative integer.");
            if (dto.Quantity < 0) throw new ArgumentOutOfRangeException(nameof(dto.Quantity), "Quantity must be a non-negative integer.");
            if (dto.Quantity == 0) dto.Quantity = 1;

            var product = await context.Products.FindAsync(dto.ProductId);
            if (product == null) throw new EntityNotFoundException("Product", dto.ProductId);

            var user = await context.Users.FindAsync(dto.UserId);
            if (user == null) throw new EntityNotFoundException("User", dto.UserId);

            //var existingOrder = await context.Orders
            //    .FirstOrDefaultAsync(o => o.UserId == dto.UserId && o.ProductId == dto.ProductId);

            //if (existingOrder != null)
            //{
            //    existingOrder.Quantity += dto.Quantity;
            //    existingOrder.CreatedDate = DateTime.UtcNow;

            //    await context.SaveChangesAsync();
            //    return existingOrder;
            //}

            var order = mapper.Map<Order>(dto);

            order.ProductName = product.Name;
            order.PriceAtPurchase = product.Price;
            order.ProductImageUrl = product.ImageUrl;
            order.CreatedDate = DateTime.UtcNow;

            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateAsync(int id, UpdateOrderDto dto)
        {
            if (id != dto.Id) throw new ArgumentException($"Route id ({id}) does not match entity Id ({dto.Id}).", nameof(id));
            if (dto.Quantity < 0) throw new ArgumentOutOfRangeException(nameof(dto.Quantity), "Quantity must be a non-negative integer.");
            if (dto.Quantity == 0) dto.Quantity++;

            var order = await context.Orders.FindAsync(id);
            if (order == null) throw new EntityNotFoundException("Order", id);

            mapper.Map(dto, order);
            order.CreatedDate = DateTime.UtcNow;

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var order = await context.Orders.FindAsync(id);
            if (order == null) throw new EntityNotFoundException("Order", id);

            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }

        public async Task AddSelectedOptionToOrderAsync(int orderId, int optionId)
        {
            if (orderId < 0) throw new ArgumentOutOfRangeException(nameof(orderId), "Order Id must be a non-negative integer.");
            if (optionId < 0) throw new ArgumentOutOfRangeException(nameof(optionId), "Option Id must be a non-negative integer.");

            var order = await context.Orders
                .Include(x => x.SelectedOptions)
                .FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null) throw new EntityNotFoundException("Order", orderId);

            var option = await context.Options
                .Include(o => o.OptionGroup)
                .FirstOrDefaultAsync(o => o.Id == optionId);
            if (option == null) throw new EntityNotFoundException("Option", optionId);

            var selectedOption = new OrderSelectedOption
            {
                OrderId = orderId,
                OptionId = option.Id,
                OptionName = option.Name,
                PriceAtPurchase = option.PriceDelta,
                GroupName = option.OptionGroup.Name
            };

            if (!order.SelectedOptions.Any(x => x.Id == optionId))
            {
                context.OrderSelectedOptions.Add(selectedOption);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveSelectedOptionFromOrderAsync(int orderId, int optionId)
        {
           if (orderId < 0) throw new ArgumentOutOfRangeException(nameof(orderId), "Order Id must be a non-negative integer.");
            if (optionId < 0) throw new ArgumentOutOfRangeException(nameof(optionId), "Option Id must be a non-negative integer.");

            var order = await GetByIdAsync(orderId);

            var option = await context.Options
                .Include(o => o.OptionGroup)
                .FirstOrDefaultAsync(o => o.Id == optionId);
            if (option == null) throw new EntityNotFoundException("Option", optionId);

            var existing = order.SelectedOptions.FirstOrDefault(x => x.Id == optionId);
            if (existing != null)
            {
                context.OrderSelectedOptions.Remove(existing);
                await context.SaveChangesAsync();
            }
        }


    }
}
