using AutoMapper;
using ClunyApi.Data;
using ClunyApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<Product>> GetAllAsync(string? filter)
        {
            var query = context.Products
                .Include(p => p.Category)
                .Include(p => p.OptionGroups)
                    .ThenInclude(g => g.Options)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var normalized = filter.Trim();

                query = query.Where(p =>
                    EF.Functions.Like(p.Name, $"%{normalized}%") ||
                    EF.Functions.Like((p.Description ?? string.Empty), $"%{normalized}%"));
            }

            return await query.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var product = await context.Products
                .Include(p => p.Category)
                .Include(p => p.OptionGroups)
                    .ThenInclude(g => g.Options)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (product == null) throw new EntityNotFoundException("Product", id);

            return product;
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var products = await context.Products
                .Include(p => p.Category)
                .Include(p => p.OptionGroups)
                    .ThenInclude(g => g.Options)
                .Where(p => p.CategoryId == id)
                .ToListAsync();

            return products;
        }

        public async Task<Product> CreateAsync(CreateProductDto dto)
        {
            if (dto.CategoryId < 0) throw new ArgumentOutOfRangeException(nameof(dto.CategoryId), "Id must be a non-negative integer.");

            var category = await context.Categories.FindAsync(dto.CategoryId);

            if (category == null) throw new EntityNotFoundException("Category", dto.CategoryId);

            var product = mapper.Map<Product>(dto);

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return product;
        }

        public async Task UpdateAsync(int id, UpdateProductDto dto)
        {
            if (id != dto.Id) throw new ArgumentException($"Route id ({id}) does not match entity Id ({dto.Id}).", nameof(id));

            if (dto.CategoryId < 0 || id < 0) throw new ArgumentOutOfRangeException("Id must be a non-negative integer.");

            var category = await context.Categories.FindAsync(dto.CategoryId);
            if (category == null) throw new EntityNotFoundException("Category", dto.CategoryId);

            var product = await context.Products.FindAsync(id);
            if (product == null) throw new EntityNotFoundException("Product", id);

            mapper.Map(dto, product);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var product = await context.Products.FindAsync(id);
            if (product == null) throw new EntityNotFoundException("Product", id);


            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task AddOptionGroupToProductAsync(int productId, int optionGroupId)
        {
            if (productId < 0) throw new ArgumentOutOfRangeException(nameof(productId));
            if (optionGroupId < 0) throw new ArgumentOutOfRangeException(nameof(optionGroupId));

            var product = await context.Products
                .Include(p => p.OptionGroups)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) throw new EntityNotFoundException("Product", productId);

            var group = await context.OptionGroups.FindAsync(optionGroupId);
            if (group == null) throw new EntityNotFoundException("OptionGroup", optionGroupId);

            if (!product.OptionGroups.Any(g => g.Id == optionGroupId))
            {
                product.OptionGroups.Add(group);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveOptionGroupFromProductAsync(int productId, int optionGroupId)
        {
            if (productId < 0) throw new ArgumentOutOfRangeException(nameof(productId));
            if (optionGroupId < 0) throw new ArgumentOutOfRangeException(nameof(optionGroupId));

            var product = await context.Products
                .Include(p => p.OptionGroups)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) throw new EntityNotFoundException("Product", productId);

            var existing = product.OptionGroups.FirstOrDefault(g => g.Id == optionGroupId);
            if (existing != null)
            {
                product.OptionGroups.Remove(existing);
                await context.SaveChangesAsync();
            }
        }


    }
}
