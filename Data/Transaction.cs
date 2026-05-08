using System;
using System.Collections.Generic;

namespace SomaShare.Data
{
    public class Transaction
    {
        public int Id { get; set; }
        public int TextbookId { get; set; }
        public virtual Textbook Textbook { get; set; } = null!;
        public int OfferId { get; set; }
        public virtual Offer Offer { get; set; } = null!;
        public string BuyerId { get; set; } = string.Empty;
        public virtual ApplicationUser Buyer { get; set; } = null!;
        public string SellerId { get; set; } = string.Empty;
        public virtual ApplicationUser Seller { get; set; } = null!;
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public bool IsComplete { get; set; } = false;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CashOnMeetup;
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }

    public enum PaymentMethod
    {
        CashOnMeetup,
        Other
    }
}