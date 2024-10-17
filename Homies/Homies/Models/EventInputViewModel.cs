using System.ComponentModel.DataAnnotations;
using static Homies.Constants.ApplicationConstants;

namespace Homies.Models
{
    public class EventInputViewModel
    {
        [Required]
        [StringLength(EventConstants.NameMaxLength, MinimumLength = EventConstants.NameMinLength, ErrorMessage = EventConstants.NameErrorMessage)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(EventConstants.DescriptionMaxLength, MinimumLength = EventConstants.DescriptionMinLength,
            ErrorMessage = EventConstants.DescriptionErrorMessage)]
        public string Description { get; set; } = string.Empty;

        [Required] public string Start { get; set; } = DateTime.Now.ToString(DateTimeFormat);

        [Required] public string End { get; set; } = DateTime.Now.ToString(DateTimeFormat);

        public int TypeId { get; set; }

        public IEnumerable<TypeSelectList> Types { get; set; } = new HashSet<TypeSelectList>();
    }
}
