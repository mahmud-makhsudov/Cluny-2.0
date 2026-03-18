using Microsoft.AspNetCore.Identity;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Dtos
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
