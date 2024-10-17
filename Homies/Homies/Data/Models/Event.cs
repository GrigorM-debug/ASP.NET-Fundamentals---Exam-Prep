using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Homies.Constants.ApplicationConstants;

namespace Homies.Data.Models
{
    public class Event
    {
        [Key]
        [Required]
        [Comment("The event unique identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(EventConstants.NameMaxLength, MinimumLength = EventConstants.NameMinLength,
            ErrorMessage = EventConstants.NameErrorMessage)]
        [Comment("The name of the event")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(EventConstants.DescriptionMaxLength, MinimumLength = EventConstants.DescriptionMinLength,
            ErrorMessage = EventConstants.DescriptionErrorMessage)]
        [Comment("The description of the event")]
        public string Description { get; set; } = null!;

        [Comment("The date and time when the event is created")]
        public DateTime CreatedOn { get; set; }

        [Comment("The date and time when the event starts")]
        public DateTime Start { get; set; }

        [Comment("The date and time when the event ends")]
        public DateTime End { get; set; }

        [Required]
        [Comment("The organizer of the event unique identifier")]
        public string OrganizerId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(OrganizerId))]
        public IdentityUser Organizer { get; set; } = null!;

        public int TypeId { get; set; }

        [Required]
        [ForeignKey(nameof(TypeId))]
        public Type Type { get; set; } = null!;

        public ICollection<EventParticipant> EventsParticipants { get; set; } = new HashSet<EventParticipant>();
    }
}
