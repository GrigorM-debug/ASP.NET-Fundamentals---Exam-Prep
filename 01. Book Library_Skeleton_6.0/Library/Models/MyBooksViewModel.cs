﻿namespace Library.Models
{
    public class MyBooksViewModel
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public decimal Rating { get; set; }

        public string Category { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
