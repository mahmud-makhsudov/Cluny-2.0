using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Timers;

namespace Shared.Models
{
    public class OrderSelectedOption
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int OptionId { get; set; }

        [JsonIgnore]
        public Option? Option { get; set; }

        // Purchase-time snapshots
        public string OptionName { get; set; } = string.Empty;
        public decimal PriceAtPurchase { get; set; }
        public string GroupName { get; set; } = string.Empty;
    }
}
