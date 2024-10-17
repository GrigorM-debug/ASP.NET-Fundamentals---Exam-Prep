using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Contacts.Constants.ApplicationUserConstants;

namespace Contacts.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        [Required]
        [Comment("The application user unique identifier")]
        public override string Id { get; set; } = null!;

        [Required]
        [StringLength(UserNameMaxLength, ErrorMessage = UserNameLengthErrorMessage, MinimumLength = UserNameMinLength)]
        [Comment("The application user username")]
        public override string UserName { get; set; } = null!;

        [Required]
        [StringLength(EmailMaxLength, ErrorMessage = EmailLengthErrorMessage, MinimumLength = EmailMinLength)]
        [Comment("The email of application user")]
        public override string Email { get; set; } = null!;

        [Required]
        [StringLength(PasswordMaxLength, ErrorMessage = PasswordErrorMessage, MinimumLength = PasswordMinLength)]
        [Comment("The application user password")]
        public string Password { get; set; } = null!;

        //public ICollection<ApplicationUserContact> ApplicationUsersContacts = new HashSet<ApplicationUserContact>();
    }
}
