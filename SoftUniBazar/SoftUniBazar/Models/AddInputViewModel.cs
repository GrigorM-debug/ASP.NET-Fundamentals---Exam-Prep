using System.ComponentModel.DataAnnotations;
using static SoftUniBazar.Constants.ApplicationConstants.AdConstants;

namespace SoftUniBazar.Models
{
    public class AddInputViewModel
    {
        [Required]
        [StringLength(NameMaxLength, ErrorMessage = NameErrorMessage, MinimumLength = NameMinLength)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(DescriptionMaxLength, ErrorMessage = DescriptionErrorMessage, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ImageUrl {get; set; } = string.Empty;

        [Required]
        [Range(typeof(decimal), "0.01", "1000000", ErrorMessage = "Please enter a valid price between 0.01 and 1,000,000.")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<CategorySelectList> Categories { get; set; } = new List<CategorySelectList>();
    }
}
