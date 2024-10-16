using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SoftUniBazar.Data.Models
{
    public class AdBuyer
    {
        [Required]
        public string BuyerId { get; set; } = null!;

        [Required]
        public IdentityUser Buyer { get; set; } = null!;

        public int AdId { get; set; }

        [Required]
        public Ad Ad { get; set; } = null!;
    }
}
