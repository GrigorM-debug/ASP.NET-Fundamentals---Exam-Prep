using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static Library.Constants.BookConstants;

namespace Library.Models
{
    public class BookInputViewModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleLengthErrorMessage)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(AuthorMaxLength, MinimumLength = AuthorMinLength, ErrorMessage = AuthorLengthErrorMessage)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

        [Required] 
        public string ImageUrl { get; set; } = string.Empty;

        [Range(RatingMinValue, RatingMaxValue, ErrorMessage = RatingRangeErrorMessage)]
        [Precision(18, 2)]
        public decimal Rating { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<CategorySelectList> Categories = new HashSet<CategorySelectList>();
    }
}
