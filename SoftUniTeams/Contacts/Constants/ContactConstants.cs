namespace Contacts.Constants
{
    public static class ContactConstants
    {
        //FirstName
        public const int FirstNameMinLength = 2;
        public const int FirstNameMaxLength = 50;
        public const string FirstNameLengthErrorMessage = "FirstName must be between {1} and {2} characters long!";

        //LastName
        public const int LastNameMinLength = 5;
        public const int LastNameMaxLength = 50;
        public const string LastNameLengthErrorMessage = "LastName must be between {1} and {2} characters long!";

        //Email
        public const int EmailMinLength = 10;
        public const int EmailMaxLength = 60;
        public const string EmailLengthErrorMessage = "Email must be between {1} and {2} characters long!";

        //Phone number
        public const int PhoneNumberMinLength = 10;
        public const int PhoneNumberMaxLength = 13;
        public const string PhoneNumberLengthErrorMessage = "Phone number must be between {1} and {2} characters long!";
        public const string PhoneNumberRegex = @"^(?:\+359|0)[ -]?\d{3}[ -]?\d{2}[ -]?\d{2}[ -]?\d{2}$";
        public const string InvalidPhoneNumberMessage = "Invalid phone number. Examples for valid phone numbers (0 875 23 45 15, +359-883-15-12-10, 0889552217)";

        //WebSite
        public const string WebSiteRegex = @"^www\.[a-zA-Z0-9-]+\.bg$";
        public const string InvalidWebsite = "Invalid Web site URl. Example for valid one (www.softuni.bg)";


    }
}
