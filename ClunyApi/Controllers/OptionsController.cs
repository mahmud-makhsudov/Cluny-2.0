using AutoMapper;
using ClunyApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OptionsController : ControllerBase
    {
        private readonly IOptionRepository optionRepository;

        public OptionsController(IOptionRepository optionRepository)
        {
            this.optionRepository = optionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Option>>> GetOptions()
        {
            var items = await optionRepository.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Option>> GetOption(int id)
        {
            var item = await optionRepository.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Option>> CreateOption(CreateOptionDto dto)
        {
            var created = await optionRepository.CreateAsync(dto);
            var resourceUrl = Url.Action(nameof(GetOption), new { id = created.Id });
            return Created(resourceUrl, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOption(int id, UpdateOptionDto dto)
        {
            await optionRepository.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOption(int id)
        {
            await optionRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
