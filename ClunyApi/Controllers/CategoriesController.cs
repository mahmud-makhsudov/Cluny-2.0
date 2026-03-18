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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(string? filter)
        {
            var items = await categoryRepository.GetAllAsync(filter);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var item = await categoryRepository.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<ActionResult<Category>> CreateCategory(string name)
        {
            var created = await categoryRepository.CreateAsync(name);
            var resourceUrl = Url.Action(nameof(GetCategory), new { id = created.Id });
            return Created(resourceUrl, created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto dto)
        {
            await categoryRepository.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await categoryRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
