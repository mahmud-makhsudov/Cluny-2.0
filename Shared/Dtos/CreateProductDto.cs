using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class CreateProductDto
    {
        
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        public int? StockQuantity { get; set; }

        public bool IsAvailable { get; set; } = true;

    }
}
