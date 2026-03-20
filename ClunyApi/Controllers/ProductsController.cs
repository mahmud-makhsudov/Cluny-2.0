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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? filter)
        {
            var items = await productRepository.GetAllAsync(filter);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var item = await productRepository.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpGet("category/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(int id)
        {
            var item = await productRepository.GetByCategoryAsync(id);
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDto dto)
        {
            var created = await productRepository.CreateAsync(dto);
            var resourceUrl = Url.Action(nameof(GetProduct), new { id = created.Id });
            return Created(resourceUrl, created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto)
        {
            await productRepository.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await productRepository.DeleteAsync(id);
            return Ok();
        }

        [HttpPost("{id}/option-groups/{optionGroupId}")]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<IActionResult> AddOptionGroup(int id, int optionGroupId)
        {
            await productRepository.AddOptionGroupToProductAsync(id, optionGroupId);
            return NoContent();
        }

        [HttpDelete("{id}/option-groups/{optionGroupId}")]
        [Authorize(Policy = AuthConstants.AdminPolicy)]
        public async Task<IActionResult> RemoveOptionGroup(int id, int optionGroupId)
        {
            await productRepository.RemoveOptionGroupFromProductAsync(id, optionGroupId);
            return NoContent();
        }
    }
}
