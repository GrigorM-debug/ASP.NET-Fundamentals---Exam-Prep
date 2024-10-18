using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static Library.Constants.CategoryConstants;

namespace Library.Data.Models
{
    public class Category
    {
        [Key]
        [Required]
        [Comment("The category unique identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameLengthErrorMessage)]
        [Comment("The name of the category")]
        public string Name { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}
