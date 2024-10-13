using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GameZone.Data
{
    public class GamerGame
    {
        public int GameId { get; set; }

        [Required]
        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; } = null!;

        [Required]
        public string GamerId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(GamerId))]
        public IdentityUser Gamer { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
