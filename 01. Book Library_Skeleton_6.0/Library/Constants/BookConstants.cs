namespace Library.Constants
{
    public class BookConstants
    {
        //Title
        public const int TitleMinLength = 10;
        public const int TitleMaxLength = 50;
        public const string TitleLengthErrorMessage = "Title must be between {2} and {1} characters long!";

        //Author
        public const int AuthorMinLength = 5;
        public const int AuthorMaxLength = 50;
        public const string AuthorLengthErrorMessage = "Author name must be between {2} and {1} characters long!";

        //Description
        public const int DescriptionMinLength = 5;
        public const int DescriptionMaxLength = 5000;
        public const string DescriptionLengthErrorMessage = "Descriotion must be between {2} and {1} characters long!";

        //Rating 
        public const double RatingMinValue = 0.00;
        public const double RatingMaxValue = 10.00;
        public const string RatingRangeErrorMessage = "Rating must be between {2} and {1}";
    }
}
