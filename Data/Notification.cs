using System.ComponentModel.DataAnnotations;

namespace SomaShare.Data
{
    /// <summary>
    /// Represents a notification for a user about platform activities
    /// (offers, transactions, reviews, etc.)
    /// </summary>
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(500)]
        public string Message { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Type { get; set; } // "Offer", "Transaction", "Review", "Message"

        public int? RelatedEntityId { get; set; } // ID of the related offer, transaction, etc.

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReadAt { get; set; }
    }
}
