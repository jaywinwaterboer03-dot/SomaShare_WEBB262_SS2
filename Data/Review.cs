using System;
using System.ComponentModel.DataAnnotations;

namespace SomaShare.Data
{
    public class Review
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; } = null!;
        public string ReviewerId { get; set; } = string.Empty;
        public virtual ApplicationUser Reviewer { get; set; } = null!;
        public string ReviewedUserId { get; set; } = string.Empty;
        public virtual ApplicationUser ReviewedUser { get; set; } = null!;
        public int Rating { get; set; } // 1-5
        [MaxLength(500)]
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
