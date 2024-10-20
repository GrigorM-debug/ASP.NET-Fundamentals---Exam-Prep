﻿using Microsoft.AspNetCore.Identity;

namespace SoftUniBazar.Models
{
    public class AdIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string CreatedOn { get; set; } = null!;

        public string Description {get; set; } = null!;

        public string Price { get; set; } = null!;

        public string Category = null!;

        public string Owner { get; set; } = null!;
    }
}
