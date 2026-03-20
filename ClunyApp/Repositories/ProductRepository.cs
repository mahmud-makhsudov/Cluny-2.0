using Shared.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace ClunyApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<ProductRepository> logger;

        public ProductRepository(IHttpClientFactory httpClientFactory, IWebHostEnvironment env, ILogger<ProductRepository> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.env = env;
            this.logger = logger;
        }

        private async Task<string> CreateImageFile(IFormFile imageFile, string? prevImgUrl = null)
        {
            var webRoot = env.WebRootPath ?? "wwwroot";
            var uploads = Path.Combine(webRoot, "uploads");
            Directory.CreateDirectory(uploads);

            if (!string.IsNullOrWhiteSpace(prevImgUrl))
            {
                try
                {
                    var prevPath = prevImgUrl;
                    if (Uri.TryCreate(prevImgUrl, UriKind.Absolute, out var uri))
                        prevPath = uri.LocalPath;

                    prevPath = prevPath.TrimStart('/', '\\');
                    var prevFullPath = Path.GetFullPath(Path.Combine(webRoot, prevPath));
                    var uploadsFullPath = Path.GetFullPath(uploads);

                    if (prevFullPath.StartsWith(uploadsFullPath, StringComparison.OrdinalIgnoreCase) && File.Exists(prevFullPath))
                    {
                        File.Delete(prevFullPath);
                    }
                }
                catch
                {
                    logger.Log(LogLevel.Warning, "Error occured during the deletion of the prev file");
                }
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploads, fileName);

            await using (var fs = File.Create(filePath))
                await imageFile.CopyToAsync(fs);

            return $"/uploads/{fileName}";
        }


        public async Task AddAsync(Product product, IFormFile? imageFile = null)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var imgPath = await CreateImageFile(imageFile);
                product.ImageUrl = imgPath;
            }


            var client = httpClientFactory.CreateClient("api");

            var productJson = new StringContent(
                JsonSerializer.Serialize(product),
                Encoding.UTF8,
                Application.Json);

            var response = await client.PostAsync("products", productJson);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id < 0) return false;

            var client = httpClientFactory.CreateClient("api");

            var response = await client.DeleteAsync($"products/{id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(responseString);
            if (doc.RootElement.TryGetProperty("success", out var successProp) && successProp.ValueKind == JsonValueKind.True)
            {
                return true;
            }
            return false;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var client = httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"products/{id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<Product>> GetByCategoryAsync(int id)
        {
            var client = httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"products/category/{id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<Product>>(
                responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (products is null) products = new List<Product>();

            return products;
        }

        public async Task<List<Product>> GetAllAsync(string? filter = null)
        {
            var client = httpClientFactory.CreateClient("api");

            var response = await client.GetAsync($"products?filter={filter}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<Product>>(
                responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (products is null) products = new List<Product>();

            return products;
        }

        public async Task<bool> UpdateAsync(Product product, IFormFile? imageFile = null)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var imgPath = await CreateImageFile(imageFile, product.ImageUrl);
                product.ImageUrl = imgPath;
            }

            var client = httpClientFactory.CreateClient("api");

            var productJson = new StringContent(
                JsonSerializer.Serialize(product),
                Encoding.UTF8,
                Application.Json);

            var response = await client.PutAsync($"products/{product.Id}", productJson);
            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task AddOptionToProduct(int productId, int optionGroupId)
        {
            var client = httpClientFactory.CreateClient("api");
            var response = await client.PostAsync($"products/{productId}/add/{optionGroupId}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveOptionFromProduct(int productId, int optionGroupId)
        {
            var client = httpClientFactory.CreateClient("api");
            var response = await client.DeleteAsync($"products/{productId}/remove/{optionGroupId}");
            response.EnsureSuccessStatusCode();
        }

    }
}
