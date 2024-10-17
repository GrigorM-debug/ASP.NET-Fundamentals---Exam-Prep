using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static Homies.Constants.ApplicationConstants;

namespace Homies.Data.Models
{
    public class Type
    {
        [Key]
        [Required]
        [Comment("The type unique identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(TypeConstants.NameMaxLength, MinimumLength = TypeConstants.NameMinLength, ErrorMessage = TypeConstants.NameErrorMessage)]
        [Comment("The name of the type")]
        public string Name { get; set; } = null!;

        public ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}
