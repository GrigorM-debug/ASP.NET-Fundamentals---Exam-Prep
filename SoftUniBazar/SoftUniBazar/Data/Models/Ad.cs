using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static SoftUniBazar.Constants.ApplicationConstants.AdConstants;

namespace SoftUniBazar.Data.Models
{
    public class Ad
    {
        [Key]
        [Required]
        [Comment("Ad unique identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, ErrorMessage = NameErrorMessage, MinimumLength = NameMinLength)]
        [Comment("The name of Ad")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, ErrorMessage = DescriptionErrorMessage, MinimumLength = DescriptionMinLength)]
        [Comment("The description of Ad")]
        public string Description { get; set; } = null!;

        [Required]
        [Precision(18,6)]
        [Range(typeof(decimal), "0.01", "1000000", ErrorMessage = "Please enter a valid price between 0.01 and 1,000,000.")]
        [Comment("The price of the Ad")]
        public decimal Price { get; set; }

        [Required]
        [Comment("The Ad image")]
        public string ImageUrl { get; set; } = null!;

        [Comment("The date and time when Ad is created")]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Comment("The Ad owner identifier")]
        public string OwnerId { get; set; } = null!;

        [Required]
        [Comment("Navigation property for Owner")]
        [ForeignKey(nameof(OwnerId))]
        public IdentityUser Owner { get; set; } = null!;

        [Comment("The Ad category unique identifier")]
        public int CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]
        [Comment("Navigation property for Category")]
        public Category Category { get; set; } = null!;
    }
}
