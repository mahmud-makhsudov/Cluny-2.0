using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Dtos
{
    public class CreateOptionDto
    {
        public int OptionGroupId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal PriceDelta { get; set; } = 0m;

        public bool IsDefault { get; set; } = false;

        public int? DisplayOrder { get; set; } = null;
    }
}
