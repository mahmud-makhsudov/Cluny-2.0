using AutoMapper;
using ClunyApi.Data;
using ClunyApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<Category>> GetAllAsync(string? filter)
        {
            var query = context.Categories
                .Include(c => c.Products)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var normalized = filter.Trim();

                query = query.Where(p => EF.Functions.Like(p.Name, $"%{normalized}%"));
            }

            return await query.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var category = await context.Categories
                .Include(c => c.Products)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (category == null) throw new EntityNotFoundException("Category", id);

            return category;
        }

        public async Task<Category> CreateAsync(string name)
        {
            var category = new Category { Name = name };

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return category;
        }

        public async Task UpdateAsync(int id, UpdateCategoryDto dto)
        {
            if (id != dto.Id) throw new ArgumentException($"Route id ({id}) does not match entity Id ({dto.Id}).", nameof(id));

            if (id < 0 || dto.Id < 0) throw new ArgumentOutOfRangeException("Id must be a non-negative integer.");

            var category = await context.Categories.FindAsync(id);
            if (category == null) throw new EntityNotFoundException("Category", id);

            mapper.Map(dto, category);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var category = await context.Categories.FindAsync(id);
            if (category == null) throw new EntityNotFoundException("Category", id);


            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }


    }
}
