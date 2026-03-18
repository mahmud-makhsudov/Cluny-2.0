using AutoMapper;
using ClunyApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptionGroupsController : ControllerBase
    {
        private readonly IOptionGroupRepository optionGroupRepository;

        public OptionGroupsController(IOptionGroupRepository optionGroupRepository)
        {
            this.optionGroupRepository = optionGroupRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OptionGroup>>> GetOptionGroups(string? filter)
        {
            var items = await optionGroupRepository.GetAllAsync(filter);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OptionGroup>> GetOptionGroup(int id)
        {
            var item = await optionGroupRepository.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<ActionResult<OptionGroup>> CreateOptionGroup(CreateOptionGroupDto dto)
        {
            var created = await optionGroupRepository.CreateAsync(dto);
            var resourceUrl = Url.Action(nameof(GetOptionGroup), new { id = created.Id });
            return Created(resourceUrl, created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<IActionResult> UpdateOptionGroup(int id, UpdateOptionGroupDto dto)
        {
            await optionGroupRepository.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<IActionResult> DeleteOptionGroup(int id)
        {
            await optionGroupRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
