using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static SoftUniBazar.Constants.ApplicationConstants.CategoryConstants;

namespace SoftUniBazar.Data.Models
{
    public class Category
    {
        [Key]
        [Required]
        [Comment("The category unique identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, ErrorMessage = NameErrorMessage, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Comment("Collection of Ads")]
        public ICollection<Ad> Ads { get; set; } = new HashSet<Ad>();
    }
}
