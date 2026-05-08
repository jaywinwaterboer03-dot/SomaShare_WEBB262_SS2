using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SomaShare.Data
{
    public class Textbook
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        [Required]
        public string ISBN { get; set; } = string.Empty;
        [Required]
        public string CourseCode { get; set; } = string.Empty;
        [Required]
        public string Condition { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Campus { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Foreign key
        public string SellerId { get; set; } = string.Empty;
        public virtual ApplicationUser Seller { get; set; } = null!;
        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<TextbookCategory> TextbookCategories { get; set; } = new List<TextbookCategory>();
    }
}
