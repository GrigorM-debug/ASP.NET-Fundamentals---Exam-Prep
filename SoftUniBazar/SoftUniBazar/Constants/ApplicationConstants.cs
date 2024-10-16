using SoftUniBazar.Data.Models;

namespace SoftUniBazar.Constants
{
    public static class ApplicationConstants
    {
        public const string DateTimeFormat = "yyyy-MM-dd H:mm";
        public const string DateTimeErrorMessage = $"Date and time must be in this format {DateTimeFormat}";

        public static class AdConstants
        {
            //Name
            public const int NameMinLength = 5;
            public const int NameMaxLength = 25;
            public const string NameErrorMessage = "Ad name must be between {1} and {2} characters long!";

            //Description
            public const int DescriptionMinLength = 15;
            public const int DescriptionMaxLength = 250;
            public const string DescriptionErrorMessage = "Description must be between {1} and {2} characters long!";
        }

        public static class CategoryConstants
        {
            //Name 
            public const int NameMinLength = 3;
            public const int NameMaxLength = 15;
            public const string NameErrorMessage = "Category name must be between {1} and {2} characters long!";
        }
    }
}
