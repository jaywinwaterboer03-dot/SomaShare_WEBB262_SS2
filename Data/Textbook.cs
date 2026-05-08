using System.ComponentModel.DataAnnotations;

namespace SomaShare.Data
{
    public class Textbook
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public string Condition { get; set; } = string.Empty;

        public string Campus { get; set; } = string.Empty;

        public string CourseCode { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string ImagePath { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string SellerId { get; set; } = string.Empty;
        public ApplicationUser? Seller { get; set; }

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public ICollection<TextbookCategory> TextbookCategories { get; set; } = new List<TextbookCategory>();
    }
}