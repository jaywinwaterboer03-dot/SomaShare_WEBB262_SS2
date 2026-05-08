using System;
using System.ComponentModel.DataAnnotations;

namespace SomaShare.Data
{
    public class Offer
    {
        public int Id { get; set; }
        public int TextbookId { get; set; }
        public virtual Textbook Textbook { get; set; } = null!;
        public string BuyerId { get; set; } = string.Empty;
        public virtual ApplicationUser Buyer { get; set; } = null!;
        public decimal OfferPrice { get; set; }
        public DateTime OfferedAt { get; set; } = DateTime.UtcNow;
        public OfferStatus Status { get; set; } = OfferStatus.Pending;
        public int? TransactionId { get; set; }
        public virtual Transaction? Transaction { get; set; }
    }

    public enum OfferStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}