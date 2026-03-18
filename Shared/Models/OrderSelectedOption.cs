using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Timers;

namespace Shared.Models
{
    public class OrderSelectedOption
    {
        public int Id { get; set; }

        public int OptionId { get; set; }

        public int OrderId { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }

        // Purchase-time snapshots
        public string OptionName { get; set; } = string.Empty;
        public decimal PriceAtPurchase { get; set; }
        public string GroupName { get; set; } = string.Empty;
    }
}
