using ClunyApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Models;

namespace ClunyApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;

        public IEnumerable<Category>? Categories { get; set; }

        public IndexModel(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }

        public async Task OnGet()
        {
            Categories = await categoryRepository.GetAllAsync();

            foreach (var category in Categories)
            {
                category.Products = await productRepository.GetByCategoryAsync(category.Id);
            }
        }
    }
}
