namespace Contacts.Constants
{
    public static class ApplicationUserConstants
    {
        //UserName
        public const int UserNameMinLength = 5;
        public const int UserNameMaxLength = 20;
        public const string UserNameLengthErrorMessage = "Username must be between {1} and {2} chatacters long!";

        //Email
        public const int EmailMinLength = 10;
        public const int EmailMaxLength = 60;
        public const string EmailLengthErrorMessage = "Email must be between {1} and {2} characters long!";

        //Password
        public const int PasswordMinLength = 5;
        public const int PasswordMaxLength = 20;
        public const string PasswordErrorMessage = "Password must be between {1} and {2} characters long!";
    }
}
