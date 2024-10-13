using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using static GameZone.Constants.ApplicationConstants.GameConstants;

namespace GameZone.Data
{
    public class Game
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(TitleMaxLength, ErrorMessage = TitleErrorMessage, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, ErrorMessage = DescriptionErrorMessage, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public DateTime ReleasedOn { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public string PublisherId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(PublisherId))]
        public IdentityUser Publisher { get; set; } = null!;

        public int GenreId { get; set; }

        [Required]
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } = null!;

        public ICollection<GamerGame> GamersGames { get; set; } = new HashSet<GamerGame>();
    }
}
