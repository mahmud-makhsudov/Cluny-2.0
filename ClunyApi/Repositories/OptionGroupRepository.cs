using AutoMapper;
using ClunyApi.Data;
using ClunyApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public class OptionGroupRepository : IOptionGroupRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public OptionGroupRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<OptionGroup>> GetAllAsync(string? filter)
        {
            var query = context.OptionGroups
                 .Include(x => x.Options)
                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                var normalized = filter.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, $"%{normalized}%") ||
                    x.Options.Any(o => EF.Functions.Like(o.Name, $"%{normalized}%")));
            }

            return await query.ToListAsync();
        }

        public async Task<OptionGroup> GetByIdAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var optionGroup = await context.OptionGroups
                .Include(x => x.Options)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (optionGroup == null) throw new EntityNotFoundException("OptionGroup", id);

            return optionGroup;
        }

        public async Task<OptionGroup> CreateAsync(CreateOptionGroupDto dto)
        {
            var optionGroup = mapper.Map<OptionGroup>(dto);

            context.OptionGroups.Add(optionGroup);
            await context.SaveChangesAsync();

            return optionGroup;
        }

        public async Task UpdateAsync(int id, UpdateOptionGroupDto dto)
        {
            if (id != dto.Id) throw new ArgumentException($"Route id ({id}) does not match entity Id ({dto.Id}).", nameof(id));

            if (id < 0 || dto.Id < 0) throw new ArgumentOutOfRangeException("Id must be a non-negative integer.");

            var optionGroup = await context.OptionGroups.FindAsync(id);
            if (optionGroup == null) throw new EntityNotFoundException("OptionGroup", id);

            mapper.Map(dto, optionGroup);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var optionGroup = await context.OptionGroups.FindAsync(id);
            if (optionGroup == null) throw new EntityNotFoundException("OptionGroup", id);


            context.OptionGroups.Remove(optionGroup);
            await context.SaveChangesAsync();
        }


    }
}
