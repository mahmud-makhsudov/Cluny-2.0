using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public string? ImageUrl { get; set; }

        public int StockQuantity { get; set; }

        public bool IsAvailable { get; set; } = true;

        public ICollection<OptionGroup> OptionGroups { get; set; } = new List<OptionGroup>();

    }
}
