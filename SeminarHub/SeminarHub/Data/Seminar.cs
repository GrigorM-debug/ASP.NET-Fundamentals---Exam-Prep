using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeminarHub.Constants;
using static SeminarHub.Constants.ApplicationConstants;
namespace SeminarHub.Data
{
    public class Seminar
    {
        [Key]
        [Required]
        [Comment("The seminar identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(SeminarConstants.TopicMaxLength, ErrorMessage = SeminarConstants.TopicErrorMessage,
            MinimumLength = SeminarConstants.TopicMinLength)]
        [Comment("The seminar topic")]
        public string Topic { get; set; } = null!;

        [Required]
        [StringLength(SeminarConstants.LecturerMaxLength, ErrorMessage = SeminarConstants.LecturerErrorMessage, MinimumLength = SeminarConstants.LecturerMinLength)]
        [Comment("The lecturer of the seminar")]
        public string Lecturer { get; set; } = null!;

        [Required]
        [StringLength(SeminarConstants.DetailsMaxLength, ErrorMessage = SeminarConstants.DetailsErrorMessage, MinimumLength = SeminarConstants.DetailsMinLength)]
        [Comment("The seminar details")]
        public string Details { get; set; } = null!;

        [Comment("The seminar date and time")]
        public DateTime DateAndTime { get; set; }

        [Range(SeminarConstants.DurationMaxValue, SeminarConstants.DurationMinValue, ErrorMessage =SeminarConstants.DurationErrorMessage)]
        [Comment("The seminar duration")]
        public int Duration { get; set; }

        [Required]
        [Comment("The seminar organizer")]
        public string OrganizerId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(OrganizerId))]
        public IdentityUser Organizer { get; set; } = null!;

        public int CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public ICollection<SeminarParticipant> SeminarsParticipants = new HashSet<SeminarParticipant>();

        public bool IsDeleted { get; set; }
    }
}
