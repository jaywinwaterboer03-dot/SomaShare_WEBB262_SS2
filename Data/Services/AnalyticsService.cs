using Microsoft.EntityFrameworkCore;

namespace SomaShare.Data.Services
{
    /// <summary>
    /// Analytics data models
    /// </summary>
    public class UserAnalyticsDto
    {
        public int TotalListingsCreated { get; set; }
        public int ActiveListings { get; set; }
        public int CompletedSales { get; set; }
        public int CompletedPurchases { get; set; }
        public decimal TotalRevenue { get; set; }
        public double AverageTrustScore { get; set; }
        public int ReviewsReceived { get; set; }
        public List<MonthlySalesDto> MonthlySales { get; set; } = new();
        public List<CategoryStatsDto> TopCategories { get; set; } = new();
    }

    public class MonthlySalesDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        public decimal Revenue { get; set; }
    }

    public class CategoryStatsDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int ListingCount { get; set; }
        public decimal AveragePrice { get; set; }
    }

    public class PlatformAnalyticsDto
    {
        public int TotalUsers { get; set; }
        public int TotalSellers { get; set; }
        public int TotalBuyers { get; set; }
        public int ActiveListings { get; set; }
        public int CompletedTransactions { get; set; }
        public decimal TotalTransactionValue { get; set; }
        public double AverageTrustScore { get; set; }
        public List<CategoryStatsDto> TopCategories { get; set; } = new();
        public List<MonthlySalesDto> MonthlySales { get; set; } = new();
    }

    /// <summary>
    /// Service for generating analytics and reports
    /// </summary>
    public interface IAnalyticsService
    {
        Task<UserAnalyticsDto> GetUserAnalyticsAsync(string userId);
        Task<PlatformAnalyticsDto> GetPlatformAnalyticsAsync();
        Task<List<ApplicationUser>> GetTopSellersAsync(int count = 10);
        Task<List<Textbook>> GetTrendingTextbooksAsync(int count = 10);
    }

    public class AnalyticsService(ApplicationDbContext context) : IAnalyticsService
    {
        public async Task<UserAnalyticsDto> GetUserAnalyticsAsync(string userId)
        {
            var user = await context.Users
                .Include(u => u.ReviewsReceived)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new UserAnalyticsDto();

            var userTextbooks = await context.Textbooks
                .Where(t => t.SellerId == userId)
                .ToListAsync();

            var completedSalesTransactions = await context.Transactions
                .Where(t => t.SellerId == userId && t.IsComplete)
                .ToListAsync();

            var completedPurchasesTransactions = await context.Transactions
                .Where(t => t.BuyerId == userId && t.IsComplete)
                .ToListAsync();

            var monthlySales = await context.Transactions
                .Where(t => t.SellerId == userId)
                .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month })
                .Select(g => new MonthlySalesDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count(),
                    Revenue = g.Where(t => t.IsComplete).Sum(t => (decimal)(t.Offer.OfferPrice))
                })
                .OrderByDescending(m => m.Year)
                .ThenByDescending(m => m.Month)
                .ToListAsync();

            var topCategories = await context.TextbookCategories
                .Include(tc => tc.Category)
                .Where(tc => tc.Textbook.SellerId == userId)
                .GroupBy(tc => tc.Category.Name)
                .Select(g => new CategoryStatsDto
                {
                    CategoryName = g.Key,
                    ListingCount = g.Count(),
                    AveragePrice = context.Textbooks
                        .Where(t => t.SellerId == userId && t.TextbookCategories.Any(tc => tc.Category.Name == g.Key))
                        .Average(t => (decimal?)t.Price) ?? 0
                })
                .OrderByDescending(c => c.ListingCount)
                .Take(5)
                .ToListAsync();

            return new UserAnalyticsDto
            {
                TotalListingsCreated = userTextbooks.Count,
                ActiveListings = userTextbooks.Count,
                CompletedSales = completedSalesTransactions.Count,
                CompletedPurchases = completedPurchasesTransactions.Count,
                TotalRevenue = completedSalesTransactions.Sum(t => t.Offer.OfferPrice),
                AverageTrustScore = user.ReviewsReceived.Count > 0 
                    ? Math.Round(user.ReviewsReceived.Average(r => r.Rating), 1)
                    : 0,
                ReviewsReceived = user.ReviewsReceived.Count,
                MonthlySales = monthlySales,
                TopCategories = topCategories
            };
        }

        public async Task<PlatformAnalyticsDto> GetPlatformAnalyticsAsync()
        {
            var totalUsers = await context.Users.CountAsync();

            var completedTransactions = await context.Transactions
                .Where(t => t.IsComplete)
                .ToListAsync();

            var allUsers = await context.Users
                .Include(u => u.ReviewsReceived)
                .ToListAsync();

            var monthlySales = await context.Transactions
                .Where(t => t.IsComplete)
                .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month })
                .Select(g => new MonthlySalesDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count(),
                    Revenue = g.Sum(t => t.Offer.OfferPrice)
                })
                .OrderByDescending(m => m.Year)
                .ThenByDescending(m => m.Month)
                .Take(12)
                .ToListAsync();

            var topCategories = await context.TextbookCategories
                .Include(tc => tc.Category)
                .GroupBy(tc => tc.Category.Name)
                .Select(g => new CategoryStatsDto
                {
                    CategoryName = g.Key,
                    ListingCount = g.Count(),
                    AveragePrice = context.Textbooks
                        .Where(t => t.TextbookCategories.Any(tc => tc.Category.Name == g.Key))
                        .Average(t => (decimal?)t.Price) ?? 0
                })
                .OrderByDescending(c => c.ListingCount)
                .Take(5)
                .ToListAsync();

            var averageTrustScore = allUsers
                .Where(u => u.ReviewsReceived.Count > 0)
                .Average(u => u.ReviewsReceived.Average(r => r.Rating));

            return new PlatformAnalyticsDto
            {
                TotalUsers = totalUsers,
                TotalSellers = await context.Textbooks.Select(t => t.SellerId).Distinct().CountAsync(),
                TotalBuyers = await context.Transactions.Select(t => t.BuyerId).Distinct().CountAsync(),
                ActiveListings = await context.Textbooks.CountAsync(),
                CompletedTransactions = completedTransactions.Count,
                TotalTransactionValue = completedTransactions.Sum(t => t.Offer.OfferPrice),
                AverageTrustScore = double.IsNaN(averageTrustScore) ? 0 : Math.Round(averageTrustScore, 1),
                TopCategories = topCategories,
                MonthlySales = monthlySales
            };
        }

        public async Task<List<ApplicationUser>> GetTopSellersAsync(int count = 10)
        {
            return await context.Users
                .Include(u => u.ReviewsReceived)
                .Where(u => u.Textbooks.Any())
                .OrderByDescending(u => u.ReviewsReceived.Average(r => (int?)r.Rating))
                .ThenByDescending(u => u.Textbooks.Count)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Textbook>> GetTrendingTextbooksAsync(int count = 10)
        {
            return await context.Textbooks
                .Include(t => t.Seller)
                .Include(t => t.Offers)
                .OrderByDescending(t => t.Offers.Count)
                .ThenByDescending(t => t.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
