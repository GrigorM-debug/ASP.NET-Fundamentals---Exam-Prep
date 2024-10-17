using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Homies.Data.Models
{
    public class EventParticipant
    {
        [Required]
        public string HelperId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(HelperId))]
        public IdentityUser Helper { get; set; } = null!;

        public int EventId { get; set; }

        [Required]
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; } = null!;
    }
}
