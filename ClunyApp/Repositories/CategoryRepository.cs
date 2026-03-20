using Shared.Models;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace ClunyApp.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IHttpClientFactory httpClientFactory;

        public CategoryRepository(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task AddAsync(Category category)
        {
            var client = httpClientFactory.CreateClient("api");

            var categoryJson = new StringContent(
                JsonSerializer.Serialize(category),
                Encoding.UTF8,
                Application.Json);

            var response = await client.PostAsync("categories", categoryJson);
            response.EnsureSuccessStatusCode();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var client = httpClientFactory.CreateClient("api");

            var response = await client.DeleteAsync($"categories/{id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(responseString);
            if (doc.RootElement.TryGetProperty("success", out var successProp) && successProp.ValueKind == JsonValueKind.True)
            {
                return true;
            }
            return false;
        }

        public async Task<Category?> GetAsync(int id)
        {
            var client = httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"categories/{id}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<Category>> GetAllAsync(string? filter = null)
        {
            var client = httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"categories?filter={filter}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<List<Category>>(
                responseString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (categories is null) categories = new List<Category>();

            return categories;
        }

        public async Task<bool> UpdateAsync(Category? category)
        {
            if (category is null) return false;

            var client = httpClientFactory.CreateClient("api");

            var categoryJson = new StringContent(
                JsonSerializer.Serialize(category),
                Encoding.UTF8,
                Application.Json);

            var response = await client.PutAsync($"categories/{category.Id}", categoryJson);
            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}
