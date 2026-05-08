using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SomaShare.Data
{
    public class WantedAd
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public string? CourseCode { get; set; }
        public string? Description { get; set; }
        public string Campus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Foreign key
        public string BuyerId { get; set; } = string.Empty;
        public virtual ApplicationUser Buyer { get; set; } = null!;
    }
}