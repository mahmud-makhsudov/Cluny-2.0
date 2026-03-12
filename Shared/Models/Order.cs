using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Timers;

namespace Shared.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTime CreatedDate { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [JsonIgnore]
        public IdentityUser? User { get; set; }

        public ICollection<OrderSelectedOption> SelectedOptions { get; set; } = new List<OrderSelectedOption>();

        // Purchase-time snapshots
        [Required]
        public string ProductName { get; set; } = string.Empty;
        [Required]
        public decimal PriceAtPurchase { get; set; }
        public string? ProductImageUrl { get; set; }
    }
}
