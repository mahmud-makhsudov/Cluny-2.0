using AutoMapper;
using ClunyApi.Data;
using ClunyApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Repositories
{
    public class OptionRepository : IOptionRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public OptionRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<Option>> GetAllAsync()
        {
            return await context.Options.ToListAsync();
        }

        public async Task<Option> GetByIdAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var option = await context.Options.FindAsync(id);

            if (option == null) throw new EntityNotFoundException("Option", id);

            return option;
        }

        public async Task<Option> CreateAsync(CreateOptionDto dto)
        {
            var optionGroup = await context.OptionGroups.FindAsync(dto.OptionGroupId);
            if (optionGroup == null) throw new EntityNotFoundException("OptionGroup", dto.OptionGroupId);

            if (dto.IsDefault)
            {
                var existingDefaults = await context.Options
                    .Where(o => o.OptionGroupId == dto.OptionGroupId && o.IsDefault)
                    .ToListAsync();

                foreach (var ex in existingDefaults)
                {
                    ex.IsDefault = false;
                }
            }

            var option = mapper.Map<Option>(dto);

            context.Options.Add(option);
            await context.SaveChangesAsync();

            return option;
        }

        public async Task UpdateAsync(int id, UpdateOptionDto dto)
        {
            if (id != dto.Id) throw new ArgumentException($"Route id ({id}) does not match entity Id ({dto.Id}).", nameof(id));

            if (id < 0 || dto.Id < 0) throw new ArgumentOutOfRangeException("Id must be a non-negative integer.");

            var option = await context.Options.FindAsync(id);
            if (option == null) throw new EntityNotFoundException("Option", id);

            var optionGroup = await context.OptionGroups.FindAsync(dto.OptionGroupId);
            if (optionGroup == null) throw new EntityNotFoundException("OptionGroup", dto.OptionGroupId);

            if (dto.IsDefault)
            {
                var existingDefaults = await context.Options
                    .Where(o => o.OptionGroupId == dto.OptionGroupId && o.IsDefault && o.Id != id)
                    .ToListAsync();

                foreach (var ex in existingDefaults)
                {
                    ex.IsDefault = false;
                }
            }

            mapper.Map(dto, option);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be a non-negative integer.");

            var option = await context.Options.FindAsync(id);
            if (option == null) throw new EntityNotFoundException("Option", id);


            context.Options.Remove(option);
            await context.SaveChangesAsync();
        }


    }
}
