namespace GameZone.Constants
{
    public static class ApplicationConstants
    {
        public const string DateFormat = "yyyy-MM-dd";
        public const string InvalidDateMessage = "Invalid Date format";

        public static class GameConstants
        {
            public const int TitleMinLength = 2;
            public const int TitleMaxLength = 50;
            public const string TitleErrorMessage = "Title must be between {1} and {2} characters long !";

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 500;
            public const string DescriptionErrorMessage = "Description must be between {1} and {2} characters long !";

        }

        public static class GenreConstants
        {
            public const int NameMaxLength = 25;
            public const int NameMinLength = 3;
            public const string NameErrorMessage = "Name must be between {1} and {2} characters long!";
        }
    }
}
