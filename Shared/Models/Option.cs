using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class Option
    {
        public int Id { get; set; }

        public int OptionGroupId { get; set; }

        [JsonIgnore]
        public OptionGroup? OptionGroup { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal PriceDelta { get; set; } = 0m;

        public bool IsDefault { get; set; } = false;

        public int? DisplayOrder { get; set; } = null;
    }
}
