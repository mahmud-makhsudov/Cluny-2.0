using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos
{
    public class UpdateOptionDto
    {
        public int Id { get; set; }

        public int OptionGroupId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal PriceDelta { get; set; } = 0m;

        public bool IsDefault { get; set; } = false;

        public int? DisplayOrder { get; set; } = null;
    }
}
