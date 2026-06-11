using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SomaShare.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Textbook> Textbooks => Set<Textbook>();
        public DbSet<WantedAd> WantedAds => Set<WantedAd>();
        public DbSet<Offer> Offers => Set<Offer>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<TextbookCategory> TextbookCategories => Set<TextbookCategory>();
        public DbSet<Favourite> Favourites => Set<Favourite>();
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasIndex(user => user.Email)
                .IsUnique();

            builder.Entity<Textbook>()
                .HasIndex(book => new { book.Title, book.Author, book.ISBN, book.CourseCode });

            builder.Entity<Textbook>()
                .Property(book => book.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Offer>()
                .Property(offer => offer.OfferPrice)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Textbook>()
                .HasOne(book => book.Seller)
                .WithMany(user => user.Textbooks)
                .HasForeignKey(book => book.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<WantedAd>()
                .HasOne(ad => ad.Buyer)
                .WithMany(user => user.WantedAds)
                .HasForeignKey(ad => ad.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Offer>()
                .HasOne(offer => offer.Buyer)
                .WithMany(user => user.Offers)
                .HasForeignKey(offer => offer.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Offer>()
                .HasOne(offer => offer.Textbook)
                .WithMany(book => book.Offers)
                .HasForeignKey(offer => offer.TextbookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Transaction>()
                .HasOne(transaction => transaction.Textbook)
                .WithMany(book => book.Transactions)
                .HasForeignKey(transaction => transaction.TextbookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(transaction => transaction.Buyer)
                .WithMany(user => user.BuyerTransactions)
                .HasForeignKey(transaction => transaction.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(transaction => transaction.Seller)
                .WithMany(user => user.SellerTransactions)
                .HasForeignKey(transaction => transaction.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasIndex(transaction => transaction.OfferId)
                .IsUnique();

            builder.Entity<Transaction>()
                .HasOne(transaction => transaction.Offer)
                .WithOne(offer => offer.Transaction)
                .HasForeignKey<Transaction>(transaction => transaction.OfferId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(review => review.Reviewer)
                .WithMany(user => user.ReviewsGiven)
                .HasForeignKey(review => review.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(review => review.ReviewedUser)
                .WithMany(user => user.ReviewsReceived)
                .HasForeignKey(review => review.ReviewedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TextbookCategory>()
                .HasKey(item => new { item.TextbookId, item.CategoryId });

            builder.Entity<TextbookCategory>()
                .HasOne(item => item.Textbook)
                .WithMany(book => book.TextbookCategories)
                .HasForeignKey(item => item.TextbookId);

            builder.Entity<TextbookCategory>()
                .HasOne(item => item.Category)
                .WithMany(category => category.TextbookCategories)
                .HasForeignKey(item => item.CategoryId);

            builder.Entity<Favourite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Favourite>()
                .HasOne(f => f.Textbook)
                .WithMany()
                .HasForeignKey(f => f.TextbookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>()
                .HasIndex(n => new { n.UserId, n.IsRead })
                .IsUnique(false);

            builder.Entity<Notification>()
                .HasIndex(n => n.CreatedAt)
                .IsUnique(false);
        }
    }
}