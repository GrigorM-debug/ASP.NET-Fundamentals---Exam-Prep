
using System.ComponentModel.DataAnnotations;
using static GameZone.Constants.ApplicationConstants.GenreConstants;

namespace GameZone.Data
{
    public class Genre
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, ErrorMessage = NameErrorMessage, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        public ICollection<Game> Games { get; set; } = new HashSet<Game>();
    }
}
