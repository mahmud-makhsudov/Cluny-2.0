using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos
{
    public class UpdateOptionGroupDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public bool IsRequired { get; set; } = false;

        public int? DisplayOrder { get; set; } = null;
    }
}
