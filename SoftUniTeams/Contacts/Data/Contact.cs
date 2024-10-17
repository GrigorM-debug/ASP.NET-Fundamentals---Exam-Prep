using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using static Contacts.Constants.ContactConstants;

namespace Contacts.Data
{
    public class Contact
    {
        [Key]
        [Required]
        [Comment("The contact unique identifier")]
        public int Id { get; set; }

        [Required]
        [StringLength(FirstNameMaxLength, ErrorMessage = FirstNameLengthErrorMessage,
            MinimumLength = FirstNameMinLength)]
        [Comment("The contact first name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, ErrorMessage = LastNameLengthErrorMessage, MinimumLength = LastNameMinLength)]
        [Comment("The contact last name")]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(EmailMaxLength, ErrorMessage = EmailLengthErrorMessage, MinimumLength = EmailMinLength)]
        [Comment("The contact email")]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(PhoneNumberRegex, ErrorMessage = InvalidPhoneNumberMessage)]
        [StringLength(PhoneNumberMaxLength, ErrorMessage = PhoneNumberLengthErrorMessage,
            MinimumLength = PhoneNumberMinLength)]
        [Comment("The contact phone number")]
        public string PhoneNumber { get; set; } = null!;

        [Comment("The address of contact")]
        public string? Address { get; set; }

        [Required]
        [RegularExpression(WebSiteRegex, ErrorMessage = InvalidWebsite)]
        [Comment("The contact web site")]
        public string Website { get; set; } = null!;

        //public ICollection<ApplicationUserContact> ApplicationsUserContacts { get; set; } =
        //    new HashSet<ApplicationUserContact>();
    }
}
