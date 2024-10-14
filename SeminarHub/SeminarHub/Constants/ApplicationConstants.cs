namespace SeminarHub.Constants
{
    public static class ApplicationConstants
    {
        public const string DateFormat = "dd/MM/yyyy HH:mm";
        public const string DateFormatErrorMessage = "Invalid DateTime format!";

        public static class CategoryConstants
        {
            //Name 
            public const int NameMaxLength = 50;
            public const int NameMinLength = 3;
            public const string NameErrorMessage = "Name must be between {1} and {2} characters long!";
        }

        public static class SeminarConstants
        {
            //Topic 
            public const int TopicMinLength = 3;
            public const int TopicMaxLength = 100;
            public const string TopicErrorMessage = "Topic must be between {1} and {2} characters long !";

            //Lecturer
            public const int LecturerMaxLength = 60;
            public const int LecturerMinLength = 5;
            public const string LecturerErrorMessage = "Lecturer name must be between {1} and {2} charecters long!";

            //Details
            public const int DetailsMaxLength = 500;
            public const int DetailsMinLength = 10;
            public const string DetailsErrorMessage = "Details must be between {1} and {2} characters long!";

            //Duration
            public const int DurationMinValue = 30;
            public const int DurationMaxValue = 180;
            public const string DurationErrorMessage = "Duration must in the range of {1} and {2} minutes!";
        }
    }
}
