using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static SeminarHub.Constants.ApplicationConstants;

namespace SeminarHub.Models
{
    public class SeminarInputViewModel
    {
        [Required]
        [StringLength(SeminarConstants.TopicMaxLength, ErrorMessage = SeminarConstants.TopicErrorMessage, MinimumLength = SeminarConstants.TopicMinLength)]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [StringLength(SeminarConstants.LecturerMaxLength, ErrorMessage = SeminarConstants.LecturerErrorMessage, MinimumLength = SeminarConstants.LecturerMinLength)]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [StringLength(SeminarConstants.DetailsMaxLength, ErrorMessage = SeminarConstants.DetailsErrorMessage, MinimumLength = SeminarConstants.DetailsMinLength)]
        public string Details { get; set; } = 
            string.Empty;

        [Required]
        public string DateAndTime { get; set; } = DateTime.Now.ToString(DateFormat);

        [Range(SeminarConstants.DurationMinValue, SeminarConstants.DurationMaxValue, ErrorMessage = SeminarConstants.DurationErrorMessage)]
        public int Duration { get; set; }

        public int CategoryId { get; set; }

        public ICollection<CategorySelectList> Categories = new HashSet<CategorySelectList>();
    }
}
