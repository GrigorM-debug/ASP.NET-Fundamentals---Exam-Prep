using System.ComponentModel.DataAnnotations;
using static Contacts.Constants.ContactConstants;

namespace Contacts.Models
{
    public class ContactInputViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(FirstNameMaxLength, ErrorMessage = FirstNameLengthErrorMessage, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(LastNameMaxLength, ErrorMessage = LastNameLengthErrorMessage, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(EmailMaxLength, ErrorMessage = EmailLengthErrorMessage, MinimumLength = EmailMinLength)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(PhoneNumberRegex, ErrorMessage = InvalidPhoneNumberMessage)]
        [StringLength(PhoneNumberMaxLength, ErrorMessage = PhoneNumberLengthErrorMessage, MinimumLength = PhoneNumberMinLength)]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? Address { get; set; }

        [Required]
        [RegularExpression(WebSiteRegex, ErrorMessage = InvalidWebsite)]
        public string Website { get; set; } = string.Empty;
    }
}
