using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Dtos
{
    public class CreateOptionGroupDto
    {

        [Required]
        public string Name { get; set; } = string.Empty;

        public bool IsRequired { get; set; } = false;

        public int? DisplayOrder { get; set; } = null;
    }
}
