using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SomaShare.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(80)]
        public string Campus { get; set; } = "Main Campus";

        [StringLength(80)]
        public string PreferredMeetupCampus { get; set; } = "Main Campus";

        [StringLength(20)]
        public string PreferredLanguage { get; set; } = "English";

        [StringLength(1000)]
        public string? Bio { get; set; }

        public ICollection<Textbook> Textbooks { get; set; } = new List<Textbook>();
        public ICollection<WantedAd> WantedAds { get; set; } = new List<WantedAd>();
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<Transaction> BuyerTransactions { get; set; } = new List<Transaction>();
        public ICollection<Transaction> SellerTransactions { get; set; } = new List<Transaction>();
        public ICollection<Review> ReviewsGiven { get; set; } = new List<Review>();
        public ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();

        public double TrustScore => ReviewsReceived.Count == 0
            ? 0
            : Math.Round(ReviewsReceived.Average(review => review.Rating), 1);
    }

}
