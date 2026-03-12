using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class OptionGroup
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        public ICollection<Option> Options { get; set; } = new List<Option>();

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public int? DisplayOrder { get; set; } = null;
    }
}
