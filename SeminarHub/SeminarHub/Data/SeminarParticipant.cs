using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SeminarHub.Data
{
    public class SeminarParticipant
    {
        public int SeminarId { get; set; }

        [Required]
        [ForeignKey(nameof(SeminarId))]
        public Seminar Seminar { get; set; } = null!;

        [Required]
        public string ParticipantId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(ParticipantId))]
        public IdentityUser Participant { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
