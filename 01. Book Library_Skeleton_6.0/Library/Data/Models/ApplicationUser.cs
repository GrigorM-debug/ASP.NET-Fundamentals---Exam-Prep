using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Library.Constants.ApplicationUserConstants;

namespace Library.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        [Required]
        [Comment("ApplicationUser unique identifier")]
        public override string Id { get; set; } = null!;

        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength, ErrorMessage = UserNameLengthErrorMessage)]
        [Comment("The ApplicationUser username")]
        public override string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength, ErrorMessage = EmailLengthErrorMessage)]
        [Comment("The email of the ApplicationUser")]
        public override string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength, ErrorMessage = PasswordLengthMessage)]
        [Comment("The password of the ApplicationUser")]
        public string Password { get; set; } = null!;
    }
}
