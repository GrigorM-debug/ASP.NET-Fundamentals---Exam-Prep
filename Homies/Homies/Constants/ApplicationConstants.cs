using System.Runtime.CompilerServices;

namespace Homies.Constants
{
    public static class ApplicationConstants
    {
        public const string DateTimeFormat = "yyyy-MM-dd H:mm";

        //DateErrorMessages
        public const string DateTimeErrorMessage = $"The date and time format must be {DateTimeFormat}";
        public const string StartDateIsRequiredMessage = "Start date is required.";
        public const string EndDateIsRequiredMessage = "End date is required.";

        //This message will be shown if End date is before Start date
        public const string EndDateIsBeforeStartDateMessage = "End date cannot be earlier than Start date.";

        //This message will be shown if Start and End date are the same
        public const string StartAndEndDateAreTheSameMessage = "Start date and End date cannot be the same.";

        public static class TypeConstants
        {
            //Name
            public const int NameMinLength = 5;
            public const int NameMaxLength = 15;
            public const string NameErrorMessage = "Name must be between {1} and {2} characters long!";
        }

        public static class EventConstants
        {
            //Name
            public const int NameMinLength = 5;
            public const int NameMaxLength = 20;

            public const string NameErrorMessage = "Name must be between {1} and {2} characters long!";
            
            //Description
            public const int DescriptionMinLength = 15;
            public const int DescriptionMaxLength = 150;
            public const string DescriptionErrorMessage = "Description must be between {1} and {2} charecters long!";
        }
    }
}
