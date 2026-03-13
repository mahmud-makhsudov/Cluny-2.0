using AutoMapper;
using ClunyApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(string? filter)
        {
            var items = await orderRepository.GetAllAsync(filter);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var item = await orderRepository.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(string name)
        {
            var created = await orderRepository.CreateAsync(name);
            var resourceUrl = Url.Action(nameof(GetCategory), new { id = created.Id });
            return Created(resourceUrl, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto dto)
        {
            await orderRepository.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await orderRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
