using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Dtos
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
