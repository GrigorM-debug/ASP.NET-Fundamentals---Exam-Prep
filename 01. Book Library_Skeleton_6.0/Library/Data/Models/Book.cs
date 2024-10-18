using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static Library.Constants.BookConstants;

namespace Library.Data.Models
{
    public class Book
    {
        [Key]
        [Required]
        [Comment("The book unique identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleLengthErrorMessage)]
        [Comment("The title of the book")]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(AuthorMaxLength, MinimumLength = AuthorMinLength, ErrorMessage = AuthorLengthErrorMessage)]
        [Comment("The author of the book")]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionLengthErrorMessage)]
        [Comment("The description of the book")]
        public string Description { get; set; } = null!;

        [Required]
        [Comment("The book image url")]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Range(RatingMinValue, RatingMaxValue, ErrorMessage = RatingRangeErrorMessage)]
        [Precision(18, 2)]
        public decimal Rating { get; set; }

        public int CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

    }
}
