using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static SeminarHub.Constants.ApplicationConstants.CategoryConstants;

namespace SeminarHub.Data
{
    public class Category
    {
        [Key]
        [Required]
        [Comment("The category identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, ErrorMessage = NameErrorMessage, MinimumLength = NameMinLength)]
        [Comment("The name of category")]
        public string Name { get; set; } = null!;

        public ICollection<Seminar> Seminars = new HashSet<Seminar>();
    }
}
