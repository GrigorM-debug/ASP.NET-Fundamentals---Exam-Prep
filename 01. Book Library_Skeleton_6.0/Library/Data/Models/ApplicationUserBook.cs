using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Data.Models
{
    public class ApplicationUserBook
    {
        [Required]
        public string ApplicationUserId { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [Required]
        public int BookId { get; set; }

        [Required]
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
