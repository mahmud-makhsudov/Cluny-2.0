using Microsoft.AspNetCore.Identity;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Dtos
{
    public class CreateOrderDto
    {

        [Required]
        public int? ProductId { get; set; }

        public int Quantity { get; set; } = 1;

        [Required]
        public string? UserId { get; set; }

    }
}
