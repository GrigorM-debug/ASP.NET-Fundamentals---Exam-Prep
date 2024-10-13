using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using GameZone.Data;

namespace GameZone.Models
{
    public class GameInputViewModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        public string? ImageUrl { get; set; } = string.Empty;

        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ReleasedOn { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");

        public int GenreId { get; set; }

        public ICollection<GenreSelectList> Genres { get; set; } = new List<GenreSelectList>();
    }

    public class GenreSelectList
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
