using Microsoft.EntityFrameworkCore;

namespace SomaShare.Data.Services
{
    /// <summary>
    /// Admin reporting models
    /// </summary>
    public class AdminReportDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int SuspendedUsers { get; set; }
        public int TotalListings { get; set; }
        public int FlaggedListings { get; set; }
        public int TotalTransactions { get; set; }
        public int PendingTransactions { get; set; }
        public decimal TotalTransactionValue { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public List<UserReportDto> RecentUsers { get; set; } = new();
        public List<TransactionReportDto> RecentTransactions { get; set; } = new();
        public List<ReviewReportDto> RecentReviews { get; set; } = new();
    }

    public class UserReportDto
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Campus { get; set; } = string.Empty;
        public double TrustScore { get; set; }
        public int ListingCount { get; set; }
        public int TransactionCount { get; set; }
        public bool IsSuspended { get; set; }
    }

    public class TransactionReportDto
    {
        public int Id { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public string SellerName { get; set; } = string.Empty;
        public string TextbookTitle { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ReviewReportDto
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public string ReviewedUserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Service for admin reporting and platform statistics
    /// </summary>
    public interface IAdminReportService
    {
        Task<AdminReportDto> GenerateFullReportAsync();
        Task<List<UserReportDto>> GetUserReportAsync(int skip = 0, int take = 50);
        Task<List<TransactionReportDto>> GetTransactionReportAsync(int skip = 0, int take = 50);
        Task<List<ReviewReportDto>> GetReviewReportAsync(int skip = 0, int take = 50);
        Task<byte[]> ExportUsersAsCsvAsync();
        Task<byte[]> ExportTransactionsAsCsvAsync();
    }

    public class AdminReportService(ApplicationDbContext context) : IAdminReportService
    {
        public async Task<AdminReportDto> GenerateFullReportAsync()
        {
            var allUsers = await context.Users
                .Include(u => u.ReviewsReceived)
                .Include(u => u.Textbooks)
                .Include(u => u.BuyerTransactions)
                .ToListAsync();

            var allTransactions = await context.Transactions
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .Include(t => t.Textbook)
                .Include(t => t.Offer)
                .ToListAsync();

            var allReviews = await context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.ReviewedUser)
                .ToListAsync();

            var activeUsers = allUsers.Count(u => u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow);
            var suspendedUsers = allUsers.Count(u => u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow);

            var recentUsers = allUsers
                .OrderByDescending(u => u.Textbooks.Count)
                .Take(5)
                .Select(u => new UserReportDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email ?? string.Empty,
                    Campus = u.Campus,
                    TrustScore = u.TrustScore,
                    ListingCount = u.Textbooks.Count,
                    TransactionCount = u.BuyerTransactions.Count + u.SellerTransactions.Count,
                    IsSuspended = u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow
                })
                .ToList();

            var recentTransactions = allTransactions
                .OrderByDescending(t => t.TransactionDate)
                .Take(10)
                .Select(t => new TransactionReportDto
                {
                    Id = t.Id,
                    BuyerName = t.Buyer?.FullName ?? "Unknown",
                    SellerName = t.Seller?.FullName ?? "Unknown",
                    TextbookTitle = t.Textbook?.Title ?? "Unknown",
                    Amount = t.Offer?.OfferPrice ?? 0,
                    IsCompleted = t.IsComplete,
                    CreatedAt = t.TransactionDate
                })
                .ToList();

            var recentReviews = allReviews
                .OrderByDescending(r => r.CreatedAt)
                .Take(10)
                .Select(r => new ReviewReportDto
                {
                    Id = r.Id,
                    ReviewerName = r.Reviewer?.FullName ?? "Unknown",
                    ReviewedUserName = r.ReviewedUser?.FullName ?? "Unknown",
                    Rating = r.Rating,
                    Comment = r.Comment ?? string.Empty,
                    CreatedAt = r.CreatedAt
                })
                .ToList();

            return new AdminReportDto
            {
                TotalUsers = allUsers.Count,
                ActiveUsers = activeUsers,
                SuspendedUsers = suspendedUsers,
                TotalListings = await context.Textbooks.CountAsync(),
                FlaggedListings = 0,
                TotalTransactions = allTransactions.Count,
                PendingTransactions = allTransactions.Count(t => !t.IsComplete),
                TotalTransactionValue = allTransactions.Where(t => t.IsComplete).Sum(t => t.Offer.OfferPrice),
                TotalReviews = allReviews.Count,
                AverageRating = allReviews.Count > 0 ? Math.Round(allReviews.Average(r => r.Rating), 1) : 0,
                RecentUsers = recentUsers,
                RecentTransactions = recentTransactions,
                RecentReviews = recentReviews
            };
        }

        public async Task<List<UserReportDto>> GetUserReportAsync(int skip = 0, int take = 50)
        {
            var users = await context.Users
                .Include(u => u.ReviewsReceived)
                .Include(u => u.Textbooks)
                .Include(u => u.BuyerTransactions)
                .OrderByDescending(u => u.Textbooks.Count)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return users
                .Select(u => new UserReportDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email ?? string.Empty,
                    Campus = u.Campus,
                    TrustScore = u.TrustScore,
                    ListingCount = u.Textbooks.Count,
                    TransactionCount = u.BuyerTransactions.Count + u.SellerTransactions.Count,
                    IsSuspended = u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow
                })
                .ToList();
        }

        public async Task<List<TransactionReportDto>> GetTransactionReportAsync(int skip = 0, int take = 50)
        {
            var transactions = await context.Transactions
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .Include(t => t.Textbook)
                .Include(t => t.Offer)
                .OrderByDescending(t => t.TransactionDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return transactions
                .Select(t => new TransactionReportDto
                {
                    Id = t.Id,
                    BuyerName = t.Buyer?.FullName ?? "Unknown",
                    SellerName = t.Seller?.FullName ?? "Unknown",
                    TextbookTitle = t.Textbook?.Title ?? "Unknown",
                    Amount = t.Offer?.OfferPrice ?? 0,
                    IsCompleted = t.IsComplete,
                    CreatedAt = t.TransactionDate
                })
                .ToList();
        }

        public async Task<List<ReviewReportDto>> GetReviewReportAsync(int skip = 0, int take = 50)
        {
            var reviews = await context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.ReviewedUser)
                .OrderByDescending(r => r.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return reviews
                .Select(r => new ReviewReportDto
                {
                    Id = r.Id,
                    ReviewerName = r.Reviewer?.FullName ?? "Unknown",
                    ReviewedUserName = r.ReviewedUser?.FullName ?? "Unknown",
                    Rating = r.Rating,
                    Comment = r.Comment ?? string.Empty,
                    CreatedAt = r.CreatedAt
                })
                .ToList();
        }

        public async Task<byte[]> ExportUsersAsCsvAsync()
        {
            var users = await GetUserReportAsync(0, int.MaxValue);
            var csv = "FullName,Email,Campus,TrustScore,ListingCount,TransactionCount,IsSuspended\n";

            foreach (var user in users)
            {
                csv += $"\"{user.FullName}\",\"{user.Email}\",\"{user.Campus}\",{user.TrustScore},{user.ListingCount},{user.TransactionCount},{user.IsSuspended}\n";
            }

            return System.Text.Encoding.UTF8.GetBytes(csv);
        }

        public async Task<byte[]> ExportTransactionsAsCsvAsync()
        {
            var transactions = await GetTransactionReportAsync(0, int.MaxValue);
            var csv = "BuyerName,SellerName,TextbookTitle,Amount,IsCompleted,CreatedAt\n";

            foreach (var transaction in transactions)
            {
                csv += $"\"{transaction.BuyerName}\",\"{transaction.SellerName}\",\"{transaction.TextbookTitle}\",{transaction.Amount},{transaction.IsCompleted},\"{transaction.CreatedAt:yyyy-MM-dd HH:mm}\"\n";
            }

            return System.Text.Encoding.UTF8.GetBytes(csv);
        }
    }
}
