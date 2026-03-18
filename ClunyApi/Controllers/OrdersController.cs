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
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(string? filter)
        {
            var items = await orderRepository.GetAllAsync(filter);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var item = await orderRepository.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Order>> GetUserOrder(string userId)
        {
            var item = await orderRepository.GetByUserAsync(userId);
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto dto)
        {
            var created = await orderRepository.CreateAsync(dto);
            var resourceUrl = Url.Action(nameof(GetOrder), new { id = created.Id });
            return Created(resourceUrl, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderDto dto)
        {
            await orderRepository.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await orderRepository.DeleteAsync(id);
            return Ok();
        }

        [HttpPost("{id}/selected-options/{optionId}")]
        public async Task<IActionResult> AddOption(int id, int optionId)
        {
            await orderRepository.AddSelectedOptionToOrderAsync(id, optionId);
            return NoContent();
        }

        [HttpDelete("{id}/selected-options/{optionId}")]
        public async Task<IActionResult> RemoveOption(int id, int optionId)
        {
            await orderRepository.RemoveSelectedOptionFromOrderAsync(id, optionId);
            return Ok();
        }
    }
}
